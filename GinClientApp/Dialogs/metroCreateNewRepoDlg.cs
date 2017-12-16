using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GinClientApp.Dialogs
{
    public partial class MetroCreateNewRepoDlg : MetroFramework.Forms.MetroForm
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        public MetroCreateNewRepoDlg()
        {
            InitializeComponent();
            SendMessage(mTxBRepoAddress.Handle, 0x1501, 1, "<username>/<repository>");
        }
    }
}
