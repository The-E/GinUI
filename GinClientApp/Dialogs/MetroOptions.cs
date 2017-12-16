using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GinClientApp.Properties;
using MetroFramework.Controls;

namespace GinClientApp.Dialogs
{
    public partial class MetroOptions : MetroFramework.Forms.MetroForm
    {
        private readonly GlobalOptions _storedOptions;

        public MetroOptions(GinApplicationContext parentContext)
        {
            InitializeComponent();

            mTxBUsername.DataBindings.Add("Text", UserCredentials.Instance, "Username");
            mTxBPassword.DataBindings.Add("Text", UserCredentials.Instance, "Password");

            mTxBDefaultCheckout.Text = GlobalOptions.Instance.DefaultCheckoutDir.FullName;
            mTxBDefaultMountpoint.Text = GlobalOptions.Instance.DefaultMountpointDir.FullName;
            mTglDownloadAnnex.Checked = GlobalOptions.Instance.RepositoryCheckoutOption ==
                                        GlobalOptions.CheckoutOption.FullCheckout;

            mTxBLicense.Text = Resources.License_Text;
            mTxBGinCliVersion.Text = parentContext.ServiceClient.GetGinCliVersion();
            mTxBGinService.Text = "";

            _storedOptions = (GlobalOptions)GlobalOptions.Instance.Clone();
        }
        

        private void mBtnOK_Click(object sender, EventArgs e)
        {
            //TODO: Data validation

            GlobalOptions.Save();
        }

        private void UpdateDefaultdir(ref DirectoryInfo directory, MetroTextBox txtBox)
        {
            var dlg = new FolderBrowserDialog
            {
                SelectedPath = directory.FullName
            };
            
            if (dlg.ShowDialog() == DialogResult.OK)
                directory = new DirectoryInfo(dlg.SelectedPath);

            txtBox.Text = directory.FullName;
        }

        private void mBtnPickDefaultCheckoutDir_Click(object sender, EventArgs e)
        {
            var directory = GlobalOptions.Instance.DefaultCheckoutDir;
            UpdateDefaultdir(ref directory, mTxBDefaultCheckout);
            GlobalOptions.Instance.DefaultCheckoutDir = directory;
        }

        private void mBtnPickDefaultMountpointDir_Click(object sender, EventArgs e)
        {
            var directory = GlobalOptions.Instance.DefaultMountpointDir;
            UpdateDefaultdir(ref directory, mTxBDefaultMountpoint);
            GlobalOptions.Instance.DefaultMountpointDir = directory;
        }

        private void mTglDownloadAnnex_CheckedChanged(object sender, EventArgs e)
        {
            GlobalOptions.Instance.RepositoryCheckoutOption = mTglDownloadAnnex.Checked
                ? GlobalOptions.CheckoutOption.FullCheckout
                : GlobalOptions.CheckoutOption.AnnexCheckout;
        }

        private void mBtnCancel_Click(object sender, EventArgs e)
        {
            GlobalOptions.Instance = _storedOptions;

            GlobalOptions.Save();
        }
    }
}
