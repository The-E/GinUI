using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceProcess;
using System.Windows.Forms;
using GinClientLibrary;
using GinShellExtension.GinService;
using Newtonsoft.Json;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using static GinClientLibrary.GinRepository;

namespace GinShellExtension
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    public class GinShellExtensionClass : SharpContextMenu
    {

        private Dictionary<string, GinRepository.FileStatus> _fileStatus = new Dictionary<string, FileStatus>();
        private IGinService _client = null;

        protected override bool CanShowMenu()
        {
            try
            {
                _client = ServiceClient.CreateServiceClient(this, 8741);
                if (!_client.IsAlive())
                    return false;

                var result = _client.IsManagedPath(SelectedItemPaths.First());

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

            if (_client == null)
                _client = ServiceClient.CreateServiceClient(this, 8741);
            _fileStatus.Clear();

            foreach (var file in SelectedItemPaths)
            {
                var fstatusstr = _client.GetFileInfo(file);
                if (Enum.TryParse(fstatusstr, out FileStatus status))
                {
                    _fileStatus.Add(file, status);
                }
            }

            try
            {
                baseItem.DropDownItems.AddRange(_client.IsBasePath(SelectedItemPaths.First())
                    ? GetBaseDirectoryMenu()
                    : GetFileMenu());

                ((ICommunicationObject)_client).Close();
            }
            catch
            {
                ((ICommunicationObject)_client).Abort();
            }

            _client = null;

            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(baseItem);
            menu.Items.Add(new ToolStripSeparator());
            return menu;
        }

        private ToolStripItem[] GetFileMenu()
        {
            if (SelectedItemPaths.Count() > 1)
            {
                var mItems = new List<ToolStripItem>
                {
                    new ToolStripMenuItem("Download Files", null, FileDownload),
                    new ToolStripMenuItem("Remove local contents", null, FileRemove),
                    new ToolStripMenuItem("Upload files", null, FileUpload)
                };
                return mItems.ToArray();
            }

            if (SelectedItemPaths.Count() == 1)
            {
                var mItems = new List<ToolStripItem>();
                var file = SelectedItemPaths.First();

                if (_fileStatus[file] == FileStatus.Directory)
                {
                    mItems.Add(new ToolStripMenuItem("Download Files", null, FileDownload));
                    mItems.Add(new ToolStripMenuItem("Remove local contents", null, FileRemove));
                    mItems.Add(new ToolStripMenuItem("Upload files", null, FileUpload));
                }

                if (_fileStatus[file] == FileStatus.InAnnex || _fileStatus[file] == FileStatus.InAnnexModified)
                {
                    mItems.Add(new ToolStripMenuItem("Download File", null, FileDownload));
                }

                if (_fileStatus[file] == FileStatus.OnDiskModified || _fileStatus[file] == FileStatus.Unknown)
                {
                    mItems.Add(new ToolStripMenuItem("Upload file", null, FileUpload));
                }

                if (_fileStatus[file] == FileStatus.OnDisk)
                {
                    mItems.Add(new ToolStripMenuItem("Remove local content", null, FileRemove));
                }

                return mItems.ToArray();
            }

            return new ToolStripItem[0];
        }

        private void FileUpload(object sender, EventArgs e)
        {
            foreach (var file in SelectedItemPaths)
                _client.UploadFile("%EMPTYSTRING%", file);
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

        private void FileRemove(object sender, EventArgs eventArgs)
        {
            _client.RemoveLocalContent(SelectedItemPaths.ToArray());
        }
    }
}