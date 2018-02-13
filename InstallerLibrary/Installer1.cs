using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace InstallerLibrary
{
    [RunInstaller(true)]
    public partial class Installer1 : Installer
    {
        private static readonly string _ginURL =
            "https://web.gin.g-node.org/G-Node/gin-cli-releases/raw/master/gin-cli-latest-windows-386.zip";

        private volatile bool _downloadComplete;

        public DirectoryInfo Path;

        public Installer1()
        {
            InitializeComponent();

            Committed += Installer1_Committed;
        }

        private void Installer1_Committed(object sender, InstallEventArgs e)
        {
            Debugger.Launch();
            var path = new DirectoryInfo(Context.Parameters["assemblypath"]).Parent;

            if (Directory.Exists(path.FullName + @"\gin-cli\"))
                Directory.Delete(path.FullName + @"\gin-cli\", true);

            Directory.CreateDirectory(path.FullName + @"\dokan\");
            Directory.CreateDirectory(path.FullName + @"\gin-cli\");

            _downloadComplete = false;
            var wb = new WebClient();
            //Download the current gin-cli release and unpack it into our install directory
            wb.DownloadFileCompleted += Wb_DownloadFileCompleted;
            wb.DownloadProgressChanged += WbOnDownloadProgressChanged;
            wb.DownloadFileAsync(new Uri(_ginURL), path.FullName + @"\gin-cli\gin-cli-latest-windows-386.zip");

            while (!_downloadComplete)
                Thread.Sleep(500);

            ZipFile.ExtractToDirectory(path.FullName + @"\gin-cli\gin-cli-latest-windows-386.zip",
                path.FullName + @"\gin-cli\");

            //Add gin-cli to the system PATH
            var value = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            value += ";" + path.FullName + @"\gin-cli\bin";
            value += ";" + path.FullName + @"\gin-cli\git\usr\bin";
            value += ";" + path.FullName + @"\gin-cli\git\bin";
            Environment.SetEnvironmentVariable("PATH", value, EnvironmentVariableTarget.Machine);

            //Give the client the ability to register a URL to communicate with the service

            var procStartInfo = new ProcessStartInfo("cmd.exe",
                "/C netsh http add urlacl url=http://+:8738/GinService/ user=\"" + Environment.UserDomainName +
                "\\" + Environment.UserName + "\" delegate=yes")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process {StartInfo = procStartInfo};
            var Output = new StringBuilder();
            process.OutputDataReceived += (o, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    Output.AppendLine(args.Data);
            };
            Output.Clear();
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();

            Output.Clear();

            //Set the dokan installer to run after reboot
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", true);
            string startPath = path.FullName + @"\dokan\DokanSetup.exe";
            regkey.SetValue("KeyName", "\"" + startPath + "\"");
        }

        private void WbOnDownloadProgressChanged(object sender,
            DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
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

            try
            {
                if (IsServiceRunning("GinClientService"))
                    StopService("GinClientService");

                if (IsServiceInstalled("GinClientService"))
                    Uninstallservice();
            }
            catch
            {
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

            try
            {
                if (IsServiceRunning("GinClientService"))
                    StopService("GinClientService");

                Uninstallservice();
            }
            catch
            {
            }

            //remove the shell extension
            var procStartInfo = new ProcessStartInfo("cmd.exe", "/C srm.exe uninstall GinShellExtension.dll -codebase")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process {StartInfo = procStartInfo};
            var Output = new StringBuilder();
            process.OutputDataReceived += (o, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    Output.AppendLine(args.Data);
            };
            Output.Clear();
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            Output.Clear();

            //clean up the system path
            var value = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);

            var entries = value.Split(';');
            var newPath = new StringBuilder();

            foreach (var entry in entries)
            {
                if (entry.Contains("gin-cli"))
                {
                    if (Directory.Exists(entry))
                        Directory.Delete(entry, true);
                    continue;
                }

                newPath.Append(entry + ";");
            }

            Environment.SetEnvironmentVariable("PATH", newPath.ToString(), EnvironmentVariableTarget.Machine);
        }

        private void Uninstallservice()
        {
            var path = new DirectoryInfo(Context.Parameters["assemblypath"]).Parent;

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
            var services = ServiceController.GetServices();
            return services.FirstOrDefault(_ =>
                string.Compare(_.ServiceName, serviceName, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public static bool IsServiceRunning(string serviceName)
        {
            ServiceControllerStatus status;
            uint counter = 0;
            do
            {
                var service = GetService(serviceName);
                if (service == null)
                    return false;

                Thread.Sleep(100);
                status = service.Status;
            } while (!(status == ServiceControllerStatus.Stopped ||
                       status == ServiceControllerStatus.Running) &&
                     ++counter < 30);
            return status == ServiceControllerStatus.Running;
        }

        public static bool IsServiceInstalled(string serviceName)
        {
            return GetService(serviceName) != null;
        }

        public static void StartService(string serviceName)
        {
            var controller = GetService(serviceName);
            if (controller == null)
                return;

            controller.Start();
            controller.WaitForStatus(ServiceControllerStatus.Running);
        }

        public static void StopService(string serviceName)
        {
            var controller = GetService(serviceName);
            if (controller == null)
                return;

            controller.Stop();
            controller.WaitForStatus(ServiceControllerStatus.Stopped);
        }
    }
}