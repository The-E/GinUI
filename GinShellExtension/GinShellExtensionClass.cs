using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceProcess;
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
        protected override bool CanShowMenu()
        {
            try
            {
                var client = ServiceClient.CreateServiceClient(this, 8741);
                if (!client.IsAlive())
                    return false;

                var result = client.IsManagedPath(SelectedItemPaths.First());
                ((ICommunicationObject) client).Close();

                return result;
            }
            catch
            {
                return false;
            }
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var baseItem = new ToolStripMenuItem("Gin Repository");

            var client = ServiceClient.CreateServiceClient(this, 8741);

            try
            {
                baseItem.DropDownItems.AddRange(client.IsBasePath(SelectedItemPaths.First())
                    ? GetBaseDirectoryMenu()
                    : GetFileMenu());

                ((ICommunicationObject) client).Close();
            }
            catch
            {
                ((ICommunicationObject) client).Abort();
            }

            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(baseItem);
            menu.Items.Add(new ToolStripSeparator());
            return menu;
        }

        private ToolStripItem[] GetFileMenu()
        {
            var mItems = new List<ToolStripItem>
            {
                new ToolStripMenuItem("Download File", null, FileDownload),
                new ToolStripMenuItem("Remove local contents", null, FileRemove)
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

        private async void RepoUpdate(object sender, EventArgs eventArgs)
        {
            var client = ServiceClient.CreateServiceClient(this, 8741);

            await client.UpdateRepositoriesAsync(SelectedItemPaths.ToArray());
            ((ICommunicationObject) client).Close();
        }

        private async void RepoUpload(object sender, EventArgs eventArgs)
        {
            var client = ServiceClient.CreateServiceClient(this, 8741);

            await client.UploadRepositoriesAsync(SelectedItemPaths.ToArray());
            ((ICommunicationObject) client).Close();
        }

        private async void FileDownload(object sender, EventArgs eventArgs)
        {
            var client = ServiceClient.CreateServiceClient(this, 8741);

            await client.DownloadFilesAsync(SelectedItemPaths.ToArray());
            ((ICommunicationObject) client).Close();
        }

        private async void FileRemove(object sender, EventArgs eventArgs)
        {
            var client = ServiceClient.CreateServiceClient(this, 8741);

            await client.RemoveLocalContentAsync(SelectedItemPaths.ToArray());
            ((ICommunicationObject)client).Close();
        }
    }
}