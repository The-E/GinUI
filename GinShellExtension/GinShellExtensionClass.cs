using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Windows.Forms;
using GinShellExtension.GinService;
using SharpShell.SharpContextMenu;

namespace GinShellExtension
{
    [ComVisible(true)]
    public class GinShellExtensionClass : SharpContextMenu
    {
        private readonly GinServiceClient _client;
        private bool _clientFaulted;

        public GinShellExtensionClass()
        {
            _client = new GinServiceClient(new InstanceContext(this));
            _client.InnerChannel.Faulted += (sender, args) => _clientFaulted = true;
        }

        protected override bool CanShowMenu()
        {
            return !_clientFaulted && _client.IsManagedPath(SelectedItemPaths.First());
        }

        protected override ContextMenuStrip CreateMenu()
        {
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
            _client.UpdateRepositories(SelectedItemPaths.ToArray());
        }

        private void RepoUpload(object sender, EventArgs eventArgs)
        {
            _client.UploadRepositories(SelectedItemPaths.ToArray());
        }

        private void FileDownload(object sender, EventArgs eventArgs)
        {
            _client.DownloadFiles(SelectedItemPaths.ToArray());
        }
    }
}
