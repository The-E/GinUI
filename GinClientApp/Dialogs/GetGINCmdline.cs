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
    public partial class GetGINCmdline : Form
    {
        public string Commandline { get; set; }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);


        public GetGINCmdline()
        {
            InitializeComponent();
            SendMessage(txtCommandLine.Handle, 0x1501, 1, "gin get <repository>");
        }

        private void txtCommandLine_TextChanged(object sender, EventArgs e)
        {
            Commandline = txtCommandLine.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Commandline)) return;
            if (!Commandline.ToLowerInvariant().Contains("gin get")) return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
