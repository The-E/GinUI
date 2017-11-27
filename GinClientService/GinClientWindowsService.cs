using System.ServiceModel;
using System.ServiceProcess;

namespace GinClientService
{
    public class GinClientWindowsService : ServiceBase
    {
        public ServiceHost serviceHost = null;
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

            serviceHost?.Close();

            // Create a ServiceHost for the CalculatorService type and 
            // provide the base address.
            serviceHost = new ServiceHost(typeof(GinClientService));

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost == null) return;
            serviceHost.Close();
            serviceHost = null;
        }
    }
}
