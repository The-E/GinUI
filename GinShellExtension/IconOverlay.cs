using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ServiceModel;
using GinShellExtension.GinService;
using GinShellExtension.Properties;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

namespace GinShellExtension
{
    [ComVisible(true)]
    public class IconOverlay : SharpIconOverlayHandler, IGinServiceCallback
    {
        private static string _path;
        private static string _status;


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
            _path = path;

            var client = ServiceClient.CreateServiceClient(this, 8743);

            try
            {
                var result = client.IsManagedPathNonTerminating(path);
                if (result)
                    _status = client.GetFileInfo(path);

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
            if (string.Compare(_status, "OnDisk", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                string.Compare(_status, "OnDiskModified", StringComparison.InvariantCultureIgnoreCase) == 0)
                return Resources.gin_icon;
            return Resources.gin_icon_desaturated;
        }
    }
}