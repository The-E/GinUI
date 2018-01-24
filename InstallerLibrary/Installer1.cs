using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Policy;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace InstallerLibrary
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        private static readonly string _dokanURL =
            "https://github.com/dokan-dev/dokany/releases/download/v1.1.0.2000/DokanSetup.exe";

        private static readonly string _ginURL =
            "https://github.com/G-Node/gin-cli/releases/download/v0.12/gin-cli-0.12-windows-386.zip";

        public Installer1()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            Debugger.Launch();

            if (IsServiceRunning("GinClientService"))
                StopService("GinClientService");

            if (IsServiceInstalled("GinClientService"))
            {
                Uninstall(stateSaver);
            }

            
            DirectoryInfo path = new DirectoryInfo(Context.Parameters["targetdir"]);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = path.FullName + "GinService.exe",
                    WorkingDirectory = path.FullName,
                    Arguments = "-install",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();

            StartService("GinClientService");

            //TODO: Download dokan installer
            WebClient wb = new WebClient();
            Directory.CreateDirectory(path.FullName + @"dokan\");
            wb.DownloadFile(_dokanURL, path.FullName + @"dokan\" + "DokanSetup.exe");

            process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = path.FullName + @"dokan\" + "DokanSetup.exe",
                    WorkingDirectory = path.FullName,
                    Arguments = "/install /quiet /norestart",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();

            //TODO: Download gin client
            Directory.CreateDirectory(path.FullName + @"gin-cli\");
            wb.DownloadFile(_ginURL, path.FullName + @"gin -cli\gin-cli-latest-windows-386.zip");

            System.IO.Compression.ZipFile.ExtractToDirectory(path.FullName + @"gin-cli\gin-cli-latest-windows-386.zip",
                path.FullName + @"gin-cli\");
            var name = "PATH";
            var value = System.Environment.GetEnvironmentVariable("PATH");
            value += ";" + path.FullName + @"gin-cli\bin";
            value += ";" + path.FullName + @"gin-cli\git\usr\bin";
            value += ";" + path.FullName + @"gin-cli\git\bin";
            System.Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Machine);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            if (IsServiceRunning("GinClientService"))
                StopService("GinClientService");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "GinService.exe",
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                    Arguments = "-uninstall",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
        }

        public static ServiceController GetService(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            return services.FirstOrDefault(_ => string.Compare(_.ServiceName, serviceName, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public static bool IsServiceRunning(string serviceName)
        {
            ServiceControllerStatus status;
            uint counter = 0;
            do
            {
                ServiceController service = GetService(serviceName);
                if (service == null)
                {
                    return false;
                }

                Thread.Sleep(100);
                status = service.Status;
            } while (!(status == ServiceControllerStatus.Stopped ||
                       status == ServiceControllerStatus.Running) &&
                     (++counter < 30));
            return status == ServiceControllerStatus.Running;
        }

        public static bool IsServiceInstalled(string serviceName)
        {
            return GetService(serviceName) != null;
        }

        public static void StartService(string serviceName)
        {
            ServiceController controller = GetService(serviceName);
            if (controller == null)
            {
                return;
            }

            controller.Start();
            controller.WaitForStatus(ServiceControllerStatus.Running);
        }

        public static void StopService(string serviceName)
        {
            ServiceController controller = GetService(serviceName);
            if (controller == null)
            {
                return;
            }

            controller.Stop();
            controller.WaitForStatus(ServiceControllerStatus.Stopped);
        }
    }
}
