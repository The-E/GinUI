using System;
using System.ServiceModel;
using System.Xml;
using GinShellExtension.GinService;

namespace GinShellExtension
{
    internal class ServiceClient
    {
        public static IGinService CreateServiceClient(object context, int port)
        {
            var iContext = new InstanceContext(context);
            var myBinding = new WSDualHttpBinding
            {
                ClientBaseAddress = new Uri(@"http://localhost:8738/GinService/ShellExtension/" + Environment.UserName + "/" + port),
                MaxBufferPoolSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                OpenTimeout = TimeSpan.FromMinutes(1.0),
                CloseTimeout = TimeSpan.FromMinutes(1.0),
                SendTimeout = TimeSpan.FromHours(1),
                ReceiveTimeout = TimeSpan.FromHours(1),
                ReaderQuotas = new XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = int.MaxValue,
                    MaxBytesPerRead = int.MaxValue,
                    MaxDepth = int.MaxValue,
                    MaxNameTableCharCount = int.MaxValue,
                    MaxStringContentLength = int.MaxValue
                }
            };
            var endpointIdentity = EndpointIdentity.CreateDnsIdentity("localhost");
            var myEndpoint = new EndpointAddress(new Uri("http://localhost:8733/GinService/"), endpointIdentity);

            var myChannelFactory = new ChannelFactory<IGinService>(myBinding, myEndpoint);

            var client = myChannelFactory.CreateChannel();
            return client;
        }
    }
}