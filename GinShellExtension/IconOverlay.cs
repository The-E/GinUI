using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceProcess;
using GinClientLibrary;
using GinShellExtension.GinService;
using GinShellExtension.Properties;
using Newtonsoft.Json;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;
using GinRepositoryData = GinClientLibrary.GinRepositoryData;

namespace GinShellExtension
{
    [ComVisible(true)]
    public class IconOverlay : SharpIconOverlayHandler, IGinServiceCallback
    {
        private static string _path;
        private Dictionary<string, GinRepository.FileStatus> _fstatus;

        //Implementing IGinServiceCallback here, but don't actually do anything with it.
        public void FileOperationStarted(string filename, string repository)
        {
        }

        public void FileOperationFinished(string filename, string repository, bool success)
        {
        }

        public void FileOperationProgress(string filename, string repository, int progress, string speed, string state)
        {
        }

        public void GinServiceError(string message)
        {
        }

        protected override int GetPriority()
        {
            return 90;
        }

        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes)
        {
            try
            {
                var sc = new ServiceController("GinClientService");
                if (sc.Status != ServiceControllerStatus.Running)
                    return false;
            }
            catch
            {
                return false;
            }

            _path = path;

            var client = ServiceClient.CreateServiceClient(this, 8743);

            try
            {
                var repos = JsonConvert.DeserializeObject<GinRepositoryData[]>(client.GetRepositoryList());
                var result = false;

                foreach (var repo in repos)
                {
                    if (!path.Contains(repo.Mountpoint.FullName)) continue;
                    result = true;
                    _fstatus = JsonConvert.DeserializeObject<
                        Dictionary<string, GinRepository.FileStatus>>(client.GetRepositoryFileInfo(repo.Name));
                    break;
                }

                ((ICommunicationObject) client).Close();

                return result;
            }
            catch
            {
                ((ICommunicationObject) client).Abort();
                return false;
            }
        }

        protected override Icon GetOverlayIcon()
        {
            if (_fstatus.ContainsKey(_path) && (_fstatus[_path] == GinRepository.FileStatus.OnDisk ||
                                                _fstatus[_path] == GinRepository.FileStatus.OnDiskModified))
                return Resources.gin_icon;
            return Resources.gin_icon_desaturated;
        }
    }
}