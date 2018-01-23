using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace GinClientApp
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            if (IsServiceRunning("GinClientService"))
                StopService("GinClientService");

            if (IsServiceInstalled("GinClientService"))
            {
                Uninstall(null);
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "GinService.exe",
                    WorkingDirectory = Directory.GetCurrentDirectory(),
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

            //TODO: Execute Dokan installer
            //TODO: Unzip gin client
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
