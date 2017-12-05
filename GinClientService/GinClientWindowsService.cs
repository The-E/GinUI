using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;

namespace GinClientService
{
    public class GinClientWindowsService : ServiceBase
    {
        private ServiceHost _serviceHost;

        public GinClientWindowsService()
        {
            // Name the Windows Service
            ServiceName = "GinClientService";
        }

        public static void Main()
        {
            Run(new GinClientWindowsService());
        }

        protected override void OnStart(string[] args)
        {
            //TODO: Remove before release!
            Debugger.Launch();

            _serviceHost?.Close();

            // CreateDirectories a ServiceHost for the CalculatorService type and 
            // provide the base address.
            _serviceHost = new ServiceHost(typeof(GinClientService));

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            _serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (_serviceHost == null) return;
            _serviceHost.Close();
            _serviceHost = null;
        }
    }
}