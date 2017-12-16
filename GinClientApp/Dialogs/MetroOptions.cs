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
using MetroFramework;
using MetroFramework.Controls;
using Newtonsoft.Json;

namespace GinClientApp.Dialogs
{
    public partial class MetroOptions : MetroFramework.Forms.MetroForm
    {
        private readonly GlobalOptions _storedOptions;
        private readonly UserCredentials _storedCredentials;
        private readonly GinApplicationContext _parentContext;

        public MetroOptions(GinApplicationContext parentContext)
        {
            InitializeComponent();

            _parentContext = parentContext;

            mLblStatus.Visible = false;
            mProgWorking.Visible = false;

            mTxBUsername.DataBindings.Add("Text", UserCredentials.Instance, "Username");
            mTxBPassword.DataBindings.Add("Text", UserCredentials.Instance, "Password");

            mTxBDefaultCheckout.Text = GlobalOptions.Instance.DefaultCheckoutDir.FullName;
            mTxBDefaultMountpoint.Text = GlobalOptions.Instance.DefaultMountpointDir.FullName;
            mTglDownloadAnnex.Checked = GlobalOptions.Instance.RepositoryCheckoutOption ==
                                        GlobalOptions.CheckoutOption.FullCheckout;

            FillRepoList();

            mTxBLicense.Text = Resources.License_Text;
            mTxBGinCliVersion.Text = parentContext.ServiceClient.GetGinCliVersion();
            mTxBGinService.Text = "";

            _storedOptions = (GlobalOptions) GlobalOptions.Instance.Clone();
            _storedCredentials = (UserCredentials) UserCredentials.Instance.Clone();
        }

        private void FillRepoList()
        {
            mLVwRepositories.Items.Clear();
            var repos = JsonConvert.DeserializeObject<GinRepositoryData[]>(_parentContext.ServiceClient.GetRepositoryList());
            foreach (var repo in repos)
            {
                mLVwRepositories.Items.Add(new ListViewItem(new[]
                    {repo.Name, repo.PhysicalDirectory.FullName, repo.Mountpoint.FullName, repo.Address}));
            }
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

        private void mBtnCheckout_Click(object sender, EventArgs e)
        {
            var repoData = new GinRepositoryData(GlobalOptions.Instance.DefaultCheckoutDir, GlobalOptions.Instance.DefaultMountpointDir, "", "", false);

            var createNewDlg = new MetroCreateNewRepoDlg(repoData);

            if (createNewDlg.ShowDialog() == DialogResult.Cancel) return;

            repoData = createNewDlg.RepositoryData;
            StartShowProgress();

            _parentContext.ServiceClient.AddRepository(repoData.PhysicalDirectory.FullName,
                repoData.Mountpoint.FullName, repoData.Name, repoData.Address,
                GlobalOptions.Instance.RepositoryCheckoutOption == GlobalOptions.CheckoutOption.FullCheckout,
                repoData.CreateNew);

            StopShowProgress();

            FillRepoList();
        }

        private void mBtnCreateNew_Click(object sender, EventArgs e)
        {
            var repoData = new GinRepositoryData(GlobalOptions.Instance.DefaultCheckoutDir, GlobalOptions.Instance.DefaultMountpointDir, "", "", true);

            var createNewDlg = new MetroCreateNewRepoDlg(repoData);

            if (createNewDlg.ShowDialog() == DialogResult.Cancel) return;

            repoData = createNewDlg.RepositoryData;
            StartShowProgress();

            _parentContext.ServiceClient.CreateNewRepository(repoData.Name);
            _parentContext.ServiceClient.AddRepository(repoData.PhysicalDirectory.FullName,
                repoData.Mountpoint.FullName, repoData.Name, repoData.Address,
                GlobalOptions.Instance.RepositoryCheckoutOption == GlobalOptions.CheckoutOption.FullCheckout,
                repoData.CreateNew);
           
            StopShowProgress();

            FillRepoList();
        }

        private void mBtnRemove_Click(object sender, EventArgs e)
        {
            if (mLVwRepositories.SelectedItems.Count == 0) return;

            var repo = mLVwRepositories.SelectedItems[0].SubItems[0].Text;
            var res = MetroMessageBox.Show(this,
                $"This will delete the repository {repo} and all associated data. Do you wish to continue?", 
                Resources.GinClientApp_Gin_Client_Warning, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (res == DialogResult.Cancel) return;

            _parentContext.ServiceClient.DeleteRepository(repo);
            FillRepoList();
        }

        private void StartShowProgress()
        {
            mLblWorking.Visible = true;
            mProgWorking.Visible = true;
            mProgWorking.Spinning = true;
        }

        private void StopShowProgress()
        {
            mLblWorking.Visible   = false;
            mProgWorking.Visible  = false;
            mProgWorking.Spinning = false;
        }

        private bool AttemptLogin()
        {
            if (!string.IsNullOrEmpty(mTxBUsername.Text) && !string.IsNullOrEmpty(mTxBPassword.Text))
            {
                _parentContext.ServiceClient.Logout();
                if (_parentContext.ServiceClient.Login(mTxBUsername.Text, mTxBPassword.Text))
                {
                    return true;
                }
            }

            return false;
        }

        private void mTxBPassword_Leave(object sender, EventArgs e)
        {
            mLblStatus.Visible = false;

            if (AttemptLogin()) return;

            mLblStatus.Text = "The entered username/password combination is not valid!";
            mLblStatus.Visible = true;
        }

        private void mTxBUsername_Leave(object sender, EventArgs e)
        {
            mLblStatus.Visible = false;

            if (AttemptLogin()) return;

            mLblStatus.Text = "The entered username/password combination is not valid!";
            mLblStatus.Visible = true;
        }
    }
}
