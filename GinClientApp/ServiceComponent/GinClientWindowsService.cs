using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using GinClientLibrary;

namespace GinService
{
    public class GinClientWindowsService
    {
        private static ServiceHost _serviceHost;

        public GinClientWindowsService()
        {
        }

        public void Start()
        {
            //Make sure to clear up any remaining user tokens
            RepositoryManager.Instance.Logout();

            _serviceHost?.Close();

            // Create a ServiceHost for the GinService type and 
            // provide the base address.
            _serviceHost = new ServiceHost(typeof(GinService));
            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            _serviceHost.Open();
        }
        
    }
}