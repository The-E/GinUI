using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace GinClientService
{
    // Provide the ProjectInstaller class which allows 
    // the service to be installed by the Installutil.exe tool
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private readonly ServiceProcessInstaller _process;
        private ServiceProcessInstaller serviceProcessInstaller1;
        private ServiceInstaller serviceInstaller1;
        private readonly ServiceInstaller _service;

        public ProjectInstaller()
        {
            _process = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            _service = new ServiceInstaller
            {
                ServiceName = "GinClientService",
                StartType = ServiceStartMode.Automatic,
            };
            
            Installers.Add(_process);
            Installers.Add(_service);
        }

        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.ServiceName = "Gin Client Service";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

        }
    }
}