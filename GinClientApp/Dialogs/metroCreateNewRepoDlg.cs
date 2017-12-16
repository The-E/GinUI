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
using GinClientLibrary;
using GinClientLibrary.Extensions;
using MetroFramework;

namespace GinClientApp.Dialogs
{
    public partial class MetroCreateNewRepoDlg : MetroFramework.Forms.MetroForm
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        public GinRepositoryData RepositoryData { get; }

        public MetroCreateNewRepoDlg(GinRepositoryData data)
        {
            InitializeComponent();

            RepositoryData = data;

            mTxBRepoName.Text = data.Name;
            mTxBRepoAddress.Text = data.Address;
            mTxBRepoCheckoutDir.Text = data.PhysicalDirectory.FullName;
            mTxBRepoMountpoint.Text = data.Mountpoint.FullName;

            SendMessage(mTxBRepoAddress.Handle, 0x1501, 1, "<username>/<repository>");
        }

        private bool CheckSanity()
        {
            if (string.IsNullOrEmpty(mTxBRepoAddress.Text))
            {
                MetroMessageBox.Show(this, "No checkout address has been entered.",
                    Resources.GinClientApp_Gin_Client_Warning, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!mTxBRepoAddress.Text.Contains('/') && !RepositoryData.CreateNew)
            {
                var result = MetroMessageBox.Show(this,
                    "The checkout address is not properly formatted. Do you wish to create a new repository with the name " +
                    mTxBRepoAddress.Text + " instead?", Resources.GinClientApp_Gin_Client_Warning,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                RepositoryData.CreateNew = result == DialogResult.Yes;
                RepositoryData.Name = RepositoryData.Address;

                return result == DialogResult.Yes;
            }

            if (Directory.Exists(RepositoryData.PhysicalDirectory.FullName))
            {
                var result = MetroMessageBox.Show(this, "The checkout directory already exists. Do you wish to delete it?", Resources.GinClientApp_Gin_Client_Warning, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    RepositoryData.PhysicalDirectory.Empty();
                    Directory.Delete(RepositoryData.PhysicalDirectory.FullName);
                }
                else return false;
            }

            if (Directory.Exists(RepositoryData.Mountpoint.FullName))
            {
                var result = MetroMessageBox.Show(this, "The mountpoint directory already exists. Do you wish to delete it?", Resources.GinClientApp_Gin_Client_Warning, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    RepositoryData.Mountpoint.Empty();
                    Directory.Delete(RepositoryData.Mountpoint.FullName);
                }
                else return false;
            }

            return true;
        }

        private void mBtnOK_Click(object sender, EventArgs e)
        {
            if (!CheckSanity()) return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void mBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void mTxBRepoName_TextChanged(object sender, EventArgs e)
        {
            RepositoryData.Name = mTxBRepoName.Text;
        }

        private void mTxBRepoAddress_TextChanged(object sender, EventArgs e)
        {
            RepositoryData.Address = mTxBRepoAddress.Text;
        }

        private void mTxBRepoAddress_Leave(object sender, EventArgs e)
        {
            if (!mTxBRepoAddress.Text.Contains('/')) return;
            
            var strings = mTxBRepoAddress.Text.Split('/');

            if (strings.Length == 2)
            {
                RepositoryData.Name = strings[1];
                if (RepositoryData.PhysicalDirectory.IsEqualTo(GlobalOptions.Instance.DefaultCheckoutDir))
                    RepositoryData.PhysicalDirectory = new DirectoryInfo(RepositoryData.PhysicalDirectory.FullName + @"\" + RepositoryData.Name);
                if (RepositoryData.Mountpoint.IsEqualTo(GlobalOptions.Instance.DefaultMountpointDir))
                    RepositoryData.Mountpoint = new DirectoryInfo(RepositoryData.Mountpoint.FullName + @"\" + RepositoryData.Name);

                mTxBRepoName.Text = RepositoryData.Name;
                mTxBRepoCheckoutDir.Text = RepositoryData.PhysicalDirectory.FullName;
                mTxBRepoMountpoint.Text = RepositoryData.Mountpoint.FullName;
            }
        }

        private void mBtnPickRepoCheckoutDir_Click(object sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog()
            {
                SelectedPath = RepositoryData.PhysicalDirectory.FullName
            };

            var res = folderBrowser.ShowDialog();

            if (res == DialogResult.OK)
            {
                RepositoryData.PhysicalDirectory = new DirectoryInfo(folderBrowser.SelectedPath);
            }

            mTxBRepoCheckoutDir.Text = RepositoryData.PhysicalDirectory.FullName;
        }

        private void mBtnPickRepoMountpointDir_Click(object sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog()
            {
                SelectedPath = RepositoryData.Mountpoint.FullName
            };

            var res = folderBrowser.ShowDialog();

            if (res == DialogResult.OK)
            {
                RepositoryData.Mountpoint = new DirectoryInfo(folderBrowser.SelectedPath);
            }

            mTxBRepoMountpoint.Text = RepositoryData.Mountpoint.FullName;
        }
    }
}
