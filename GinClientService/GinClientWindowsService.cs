using System.ServiceModel;
using System.ServiceProcess;

namespace GinClientService
{
    public class GinClientWindowsService : ServiceBase
    {
        private ServiceHost _serviceHost = null;
        public GinClientWindowsService()
        {
            // Name the Windows Service
            ServiceName = "GinClientService";
        }

        public static void Main()
        {
            ServiceBase.Run(new GinClientWindowsService());
        }

        protected override void OnStart(string[] args)
        {
            //TODO: Remove before release!
            System.Diagnostics.Debugger.Launch();

            _serviceHost?.Close();

            // Create a ServiceHost for the CalculatorService type and 
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
