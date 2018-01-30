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
        private readonly ServiceInstaller _service;
        private ServiceInstaller _serviceInstaller1;
        private ServiceProcessInstaller _serviceProcessInstaller1;

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
            _serviceProcessInstaller1 = new ServiceProcessInstaller();
            _serviceInstaller1 = new ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            _serviceProcessInstaller1.Account = ServiceAccount.LocalService;
            _serviceProcessInstaller1.Password = null;
            _serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            _serviceInstaller1.ServiceName = "GinClientService";
            // 
            // ProjectInstaller
            // 
            Installers.AddRange(new Installer[]
            {
                _serviceProcessInstaller1,
                _serviceInstaller1
            });
        }
    }
}