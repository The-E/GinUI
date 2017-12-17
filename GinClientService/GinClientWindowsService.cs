using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using GinClientLibrary;

namespace GinService
{
    public class GinClientWindowsService : ServiceBase
    {
        private ServiceHost _serviceHost;

        public GinClientWindowsService()
        {
            // Name the Windows Service
            ServiceName = "GinService";
        }

        public static void Main()
        {
            Run(new GinClientWindowsService());
        }

        protected override void OnStart(string[] args)
        {
            //TODO: Remove before release!
            Debugger.Launch();

            RepositoryManager.Instance.Logout();

            _serviceHost?.Close();

            // Create a ServiceHost for the GinService type and 
            // provide the base address.
            _serviceHost = new ServiceHost(typeof(GinService));

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            _serviceHost.Open();
        }

        protected override void OnStop()
        {
            RepositoryManager.Instance.UnmountAllRepositories();
            RepositoryManager.Instance.Logout();

            if (_serviceHost == null) return;
            _serviceHost.Close();
            _serviceHost = null;
        }

        private void InitializeComponent()
        {
            // 
            // GinClientWindowsService
            // 
            ServiceName = "Gin Client Service";

        }
    }
}