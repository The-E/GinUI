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
                StartType = ServiceStartMode.Boot,
            };
            
            Installers.Add(_process);
            Installers.Add(_service);
        }
    }
}