using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GinShellExtension.GinService;
using SharpShell.SharpContextMenu;

namespace GinShellExtension
{
    [ComVisible(true)]
    public class GinShellExtension : SharpContextMenu
    {
        private GinServiceClient _client;

        public GinShellExtension()
        {
            _client = new GinServiceClient(new InstanceContext(this));
        }

        protected override bool CanShowMenu()
        {
            return _client.IsManagedPath(SelectedItemPaths.First());
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var baseItem = new ToolStripMenuItem("Gin Repository");

            

            return menu;
        }
    }
}
