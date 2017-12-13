using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Windows.Forms;
using GinShellExtension.GinService;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace GinShellExtension
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    public class GinShellExtensionClass : SharpContextMenu
    {
        private bool _clientFaulted;
        

        protected override bool CanShowMenu()
        {
            
            var myBinding = new WSDualHttpBinding();
            var myEndpoint = new EndpointAddress("http://localhost:8733/Design_Time_Addresses/GinService/");
            var myChannelFactory = new ChannelFactory<IGinService>(myBinding, myEndpoint);

            var client = myChannelFactory.CreateChannel();

            try
            {
                var result = client.IsManagedPath(SelectedItemPaths.First());

                ((ICommunicationObject) client).Close();

                return result;
            }
            catch
            {
                ((ICommunicationObject)client).Abort();
                return false;
            }
        }

        protected override ContextMenuStrip CreateMenu()
        {

            var menu = new ContextMenuStrip();

            var baseItem = new ToolStripMenuItem("Gin Repository");

            
            var myBinding = new WSDualHttpBinding();
            var myEndpoint = new EndpointAddress("http://localhost:8733/Design_Time_Addresses/GinService/");
            var myChannelFactory = new ChannelFactory<IGinService>(myBinding, myEndpoint);

            var client = myChannelFactory.CreateChannel();

            try
            {
                baseItem.DropDownItems.AddRange(client.IsBasePath(SelectedItemPaths.First())
                    ? GetBaseDirectoryMenu()
                    : GetFileMenu());

                ((ICommunicationObject) client).Close();
            }
            catch
            {
                ((ICommunicationObject)client).Abort();
            }
            return menu;
        }

        private ToolStripItem[] GetFileMenu()
        {
            var mItems = new List<ToolStripItem>
            {
                new ToolStripMenuItem("Download File", null, FileDownload),
            };

            return mItems.ToArray();
        }

        private ToolStripItem[] GetBaseDirectoryMenu()
        {
            var mItems = new List<ToolStripItem>
            {
                new ToolStripMenuItem("Update Repository", null, RepoUpdate),
                new ToolStripMenuItem("Upload Changes", null, RepoUpload)
            };

            return mItems.ToArray();
        }

        private void RepoUpdate(object sender, EventArgs eventArgs)
        {
            var myBinding = new WSDualHttpBinding();
            var myEndpoint = new EndpointAddress("http://localhost:8733/Design_Time_Addresses/GinService/");
            var myChannelFactory = new ChannelFactory<IGinService>(myBinding, myEndpoint);

            var client = myChannelFactory.CreateChannel();

            client.UpdateRepositories(SelectedItemPaths.ToArray());
            ((ICommunicationObject) client).Close();
        }

        private void RepoUpload(object sender, EventArgs eventArgs)
        {
            var myBinding = new WSDualHttpBinding();
            var myEndpoint = new EndpointAddress("http://localhost:8733/Design_Time_Addresses/GinService/");
            var myChannelFactory = new ChannelFactory<IGinService>(myBinding, myEndpoint);

            var client = myChannelFactory.CreateChannel();

            client.UploadRepositories(SelectedItemPaths.ToArray());
            ((ICommunicationObject)client).Close();
        }

        private void FileDownload(object sender, EventArgs eventArgs)
        {
            var myBinding = new WSDualHttpBinding();
            var myEndpoint = new EndpointAddress("http://localhost:8733/Design_Time_Addresses/GinService/");
            var myChannelFactory = new ChannelFactory<IGinService>(myBinding, myEndpoint);

            var client = myChannelFactory.CreateChannel();

            client.DownloadFiles(SelectedItemPaths.ToArray());
            ((ICommunicationObject)client).Close();
        }
    }
}
