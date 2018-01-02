using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace GinService
{
    // Provide the ProjectInstaller class which allows 
    // the service to be installed by the Installutil.exe tool
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private readonly ServiceProcessInstaller _process;
        private ServiceProcessInstaller _serviceProcessInstaller1;
        private ServiceInstaller _serviceInstaller1;
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
                Description = "Provides background services for the Gin infrastructure",
                StartType = ServiceStartMode.Automatic
            };
            
            Installers.Add(_process);
            Installers.Add(_service);
        }

        private void InitializeComponent()
        {
            this._serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this._serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this._serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this._serviceProcessInstaller1.Password = null;
            this._serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this._serviceInstaller1.ServiceName = "Gin Client Service";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this._serviceProcessInstaller1,
            this._serviceInstaller1});

        }
    }
}