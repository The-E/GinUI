using System.Drawing;
using System.Runtime.InteropServices;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

namespace GinShellExtension
{
    [ComVisible(true)]
    public class IconOverlay : SharpIconOverlayHandler
    {
        protected override int GetPriority()
        {
            return 90;
        }

        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes)
        {
            return false;
        }

        protected override Icon GetOverlayIcon()
        {
            return null;
        }
    }
}