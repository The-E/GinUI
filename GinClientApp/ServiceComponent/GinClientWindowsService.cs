using System.ServiceModel;
using System.Windows.Forms;
using GinClientLibrary;

namespace GinService
{
    public class GinClientWindowsService
    {
        private static ServiceHost _serviceHost;
        private static NotifyIcon _appIcon;

        public GinClientWindowsService(NotifyIcon appIcon)
        {
            _appIcon = appIcon;
        }

        public void Start()
        {
            //Make sure to clear up any remaining user tokens
            RepositoryManager.Instance.Logout();
            RepositoryManager.Instance.AppIcon = _appIcon;

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