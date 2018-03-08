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
            _fileStatus.Clear();

            foreach (var file in SelectedItemPaths)
            {
                var fstatusstr = client.GetFileInfo(file);
                if (Enum.TryParse(fstatusstr, out FileStatus status))
                {
                    _fileStatus.Add(file, status);
                }
            }

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
            var client = ServiceClient.CreateServiceClient(this, 8741);

            foreach (var file in SelectedItemPaths) 
                client.UploadFile("%EMPTYSTRING%", file);
            ((ICommunicationObject)client).Close();
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
            var client = ServiceClient.CreateServiceClient(this, 8741);

            client.UpdateRepositories(SelectedItemPaths.ToArray());
            ((ICommunicationObject) client).Close();
        }

        private void RepoUpload(object sender, EventArgs eventArgs)
        {
            var client = ServiceClient.CreateServiceClient(this, 8741);

            client.UploadRepositories(SelectedItemPaths.ToArray());
            ((ICommunicationObject) client).Close();
        }

        private void FileDownload(object sender, EventArgs eventArgs)
        {
            var client = ServiceClient.CreateServiceClient(this, 8741);

            client.DownloadFiles(SelectedItemPaths.ToArray());
            ((ICommunicationObject) client).Close();
        }

        private void FileRemove(object sender, EventArgs eventArgs)
        {
            var client = ServiceClient.CreateServiceClient(this, 8741);

            client.RemoveLocalContent(SelectedItemPaths.ToArray());
            ((ICommunicationObject)client).Close();
        }
    }
}