using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GinShellExtension.GinService;

namespace GinShellExtension
{
    internal class ServiceClient
    {
        public static IGinService CreateServiceClient(object context, int port)
        {
            var iContext = new InstanceContext(context);
            var myBinding = new WSDualHttpBinding() { ClientBaseAddress = new Uri(@"http://localhost:" + port + "/Design_Time_Addresses/GinService/") };
            var myEndpoint = new EndpointAddress("http://localhost:8733/Design_Time_Addresses/GinService/");
            var myChannelFactory = new DuplexChannelFactory<IGinService>(iContext, myBinding, myEndpoint);

            var client = myChannelFactory.CreateChannel();
            return client;
        }
    }
}
