using System;
using System.IO;
using System.Windows.Forms;
using GinClientApp.Properties;
using GinClientLibrary;
using MetroFramework;
using MetroFramework.Controls;
using Newtonsoft.Json;

namespace GinClientApp.Dialogs
{
    public partial class MetroOptionsDlg : MetroFramework.Forms.MetroForm
    {
        private readonly GlobalOptions _storedOptions;
        private readonly UserCredentials _storedCredentials;
        private readonly GinApplicationContext _parentContext;

        public enum Page
        {
            Login,
            GlobalOptions,
            Repositories,
            About
        }

        public MetroOptionsDlg(GinApplicationContext parentContext, Page startPage)
        {
            InitializeComponent();

            _parentContext = parentContext;

            mTabCtrl.TabIndex = (int) startPage;

            mLblStatus.Visible = false;
            mLblWorking.Visible = false;
            mProgWorking.Visible = false;

            mTxBUsername.DataBindings.Add("Text", UserCredentials.Instance, "Username");
            mTxBPassword.DataBindings.Add("Text", UserCredentials.Instance, "Password");

            mTxBDefaultCheckout.Text = GlobalOptions.Instance.DefaultCheckoutDir.FullName;
            mTxBDefaultMountpoint.Text = GlobalOptions.Instance.DefaultMountpointDir.FullName;
            mTglDownloadAnnex.Checked = GlobalOptions.Instance.RepositoryCheckoutOption ==
                                        GlobalOptions.CheckoutOption.FullCheckout;
            switch (GlobalOptions.Instance.RepositoryUpdateInterval)
            {
                case 0:
                    mCBxRepoUpdates.SelectedIndex = 0;
                    break;
                case 5:
                    mCBxRepoUpdates.SelectedIndex = 1;
                    break;
                case 15:
                    mCBxRepoUpdates.SelectedIndex = 2;
                    break;
                case 30:
                    mCBxRepoUpdates.SelectedIndex = 3;
                    break;
                case 60:
                    mCBxRepoUpdates.SelectedIndex = 4;
                    break;
                default:
                    mCBxRepoUpdates.SelectedIndex = 0;
                    break;
            }

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
            mLVwRepositories.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }


        private void mBtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            GlobalOptions.Save();
            UserCredentials.Save();

            SaveRepoList();

            Close();
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
            DialogResult = DialogResult.Cancel;

            GlobalOptions.Instance = _storedOptions;
            UserCredentials.Instance = _storedCredentials;

            GlobalOptions.Save();
            UserCredentials.Save();

            Close();
        }

        private async void mBtnCheckout_Click(object sender, EventArgs e)
        {
            var repoData = new GinRepositoryData(GlobalOptions.Instance.DefaultCheckoutDir, GlobalOptions.Instance.DefaultMountpointDir, "", "", false);

            var createNewDlg = new MetroCreateNewRepoDlg(repoData, _parentContext);

            if (createNewDlg.ShowDialog() == DialogResult.Cancel) return;

            repoData = createNewDlg.RepositoryData;
            StartShowProgress();

            var result = await _parentContext.ServiceClient.AddRepositoryAsync(repoData.PhysicalDirectory.FullName,
                repoData.Mountpoint.FullName, repoData.Name, repoData.Address,
                GlobalOptions.Instance.RepositoryCheckoutOption == GlobalOptions.CheckoutOption.FullCheckout,
                repoData.CreateNew);

            StopShowProgress();

            FillRepoList();

            SaveRepoList();
        }

        private void SaveRepoList()
        {
            var repos = JsonConvert.DeserializeObject<GinRepositoryData[]>(_parentContext.ServiceClient.GetRepositoryList());

            var saveFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                           @"\g-node\GinWindowsClient\SavedRepositories.json";

            if (!Directory.Exists(Path.GetDirectoryName(saveFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(saveFile));

            if (File.Exists(saveFile))
                File.Delete(saveFile);


            var fs = File.CreateText(saveFile);
            fs.Write(JsonConvert.SerializeObject(repos));
            fs.Flush();
            fs.Close();
        }

        private void mBtnCreateNew_Click(object sender, EventArgs e)
        {
            var repoData = new GinRepositoryData(GlobalOptions.Instance.DefaultCheckoutDir, GlobalOptions.Instance.DefaultMountpointDir, "", "", true);

            var createNewDlg = new MetroCreateNewRepoDlg(repoData, _parentContext);

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
                string.Format(Resources.Options_This_will_delete_the_repository, repo), 
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

            mLblStatus.Text = Resources.GetUserCredentials_The_entered_Username_Password_combination_is_invalid;
            mLblStatus.Visible = true;
        }

        private void mTxBUsername_Leave(object sender, EventArgs e)
        {
            mLblStatus.Visible = false;

            if (AttemptLogin()) return;

            mLblStatus.Text = Resources.GetUserCredentials_The_entered_Username_Password_combination_is_invalid;
            mLblStatus.Visible = true;
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (mCBxRepoUpdates.SelectedIndex)
            {
                case 0:
                    GlobalOptions.Instance.RepositoryUpdateInterval = 0;
                    break;
                case 1:
                    GlobalOptions.Instance.RepositoryUpdateInterval = 5;
                    break;
                case 2:
                    GlobalOptions.Instance.RepositoryUpdateInterval = 15;
                    break;
                case 3:
                    GlobalOptions.Instance.RepositoryUpdateInterval = 30;
                    break;
                case 4:
                    GlobalOptions.Instance.RepositoryUpdateInterval = 60;
                    break;
                default:
                    GlobalOptions.Instance.RepositoryUpdateInterval = 0;
                    break;
            }
        }
    }
}
