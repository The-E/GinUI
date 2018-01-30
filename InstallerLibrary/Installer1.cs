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
        private static readonly string _ginURL =
            "https://web.gin.g-node.org/G-Node/gin-cli-releases/raw/master/gin-cli-latest-windows-386.zip";

        public DirectoryInfo Path;

        public Installer1() : base()
        {
            InitializeComponent();

            this.Committed += Installer1_Committed;
        }

        private volatile bool _downloadComplete;

        private void Installer1_Committed(object sender, InstallEventArgs e)
        {
            DirectoryInfo path = new DirectoryInfo(Context.Parameters["assemblypath"]).Parent;

            Directory.CreateDirectory(path.FullName + @"\dokan\");
            Directory.CreateDirectory(path.FullName + @"\gin-cli\");

            _downloadComplete = false;
            var wb = new WebClient();
            wb.DownloadFileCompleted += Wb_DownloadFileCompleted;
            wb.DownloadProgressChanged += WbOnDownloadProgressChanged;
            wb.DownloadFileAsync(new Uri(_ginURL), path.FullName + @"\gin-cli\gin-cli-latest-windows-386.zip");

            while (!_downloadComplete)
                Thread.Sleep(500);

            System.IO.Compression.ZipFile.ExtractToDirectory(path.FullName + @"\gin-cli\gin-cli-latest-windows-386.zip",
                path.FullName + @"\gin-cli\");
            var name = "PATH";
            var value = System.Environment.GetEnvironmentVariable("PATH");
            value += ";" + path.FullName + @"\gin-cli\bin";
            value += ";" + path.FullName + @"\gin-cli\git\usr\bin";
            value += ";" + path.FullName + @"\gin-cli\git\bin";
            System.Environment.SetEnvironmentVariable("PATH", value, EnvironmentVariableTarget.Machine);

            string arguments;
            var domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            if (string.IsNullOrEmpty(domain))
            {
                arguments =
                    @"http add urlacl url=http://+:8738/Design_Time_Addresses/GinService/ user=%COMPUTERNAME%\%USERNAME%";
            }
            else
            {
                arguments = @"http add urlacl url=http://+:8738/Design_Time_Addresses/GinService/ user=" + domain +
                            @"\%USERNAME%";
            }
            ProcessStartInfo procStartInfo = new ProcessStartInfo("netsh", arguments);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            Process.Start(procStartInfo);

            StartService("GinClientService");
            Process.Start(path.FullName + @"\GinClientApp.exe");
        }

        private void WbOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            Console.WriteLine(downloadProgressChangedEventArgs.ProgressPercentage);
        }

        private void Wb_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _downloadComplete = true;
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            
            if (IsServiceRunning("GinClientService"))
                StopService("GinClientService");

            if (IsServiceInstalled("GinClientService"))
            {
                Uninstallservice();
            }
            
            Path = new DirectoryInfo(Context.Parameters["targetdir"]);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = Path.FullName + "GinService.exe",
                    WorkingDirectory = Path.FullName,
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

            Uninstallservice();
        }

        private void Uninstallservice()
        {
            DirectoryInfo path = new DirectoryInfo(Context.Parameters["assemblypath"]).Parent;

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = path.FullName + "GinService.exe",
                    WorkingDirectory = path.FullName,
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
