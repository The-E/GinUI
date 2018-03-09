using System;
using MetroFramework.Forms;

namespace GinClientApp.Dialogs
{
    public partial class DeleteDataDlg : MetroForm
    {
        public DeleteDataDlg()
        {
            InitializeComponent();

            chB_keepCheckouts.Checked = KeepCheckout;
            chB_keepLoginInfo.Checked = KeepUserLogin;
            chB_keepUserConfig.Checked = KeepUserConfig;
        }

        public bool KeepCheckout { get; set; }
        public bool KeepUserConfig { get; set; }
        public bool KeepUserLogin { get; set; }

        private void DeleteDataDlg_Load(object sender, EventArgs e)
        {
        }

        private void chB_keepUserConfig_CheckedChanged(object sender, EventArgs e)
        {
            KeepUserConfig = chB_keepUserConfig.Checked;
        }

        private void chB_keepLoginInfo_CheckedChanged(object sender, EventArgs e)
        {
            KeepUserLogin = chB_keepLoginInfo.Checked;
        }

        private void chB_keepCheckouts_CheckedChanged(object sender, EventArgs e)
        {
            KeepCheckout = chB_keepCheckouts.Checked;
        }
    }
}