using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

namespace GinShellExtension
{
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
