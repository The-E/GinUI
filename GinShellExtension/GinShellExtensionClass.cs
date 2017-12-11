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
            var _client = new GinServiceClient(new InstanceContext(this));
            _client.InnerChannel.Faulted += (sender, args) => _clientFaulted = true;

            return !_clientFaulted && _client.IsManagedPath(SelectedItemPaths.First());
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var _client = new GinServiceClient(new InstanceContext(this));
            _client.InnerChannel.Faulted += (sender, args) => _clientFaulted = true;

            var menu = new ContextMenuStrip();

            var baseItem = new ToolStripMenuItem("Gin Repository");

            baseItem.DropDownItems.AddRange(_client.IsBasePath(SelectedItemPaths.First())
                ? GetBaseDirectoryMenu()
                : GetFileMenu());

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
            var _client = new GinServiceClient(new InstanceContext(this));
            _client.InnerChannel.Faulted += (sender1, args) => _clientFaulted = true;

            _client.UpdateRepositories(SelectedItemPaths.ToArray());
        }

        private void RepoUpload(object sender, EventArgs eventArgs)
        {
            var _client = new GinServiceClient(new InstanceContext(this));
            _client.InnerChannel.Faulted += (sender1, args) => _clientFaulted = true;

            _client.UploadRepositories(SelectedItemPaths.ToArray());
        }

        private void FileDownload(object sender, EventArgs eventArgs)
        {
            var _client = new GinServiceClient(new InstanceContext(this));
            _client.InnerChannel.Faulted += (sender1, args) => _clientFaulted = true;

            _client.DownloadFiles(SelectedItemPaths.ToArray());
        }
    }
}
