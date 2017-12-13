using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using GinClientApp.GinService;
using GinClientApp.Properties;
using GinClientLibrary;
using Newtonsoft.Json;

namespace GinClientApp.Dialogs
{
    public partial class RepoManagement : Form
    {
        private GinServiceClient _client;
        private readonly GinApplicationContext.UserCredentials _credentials;
        private readonly GinApplicationContext.GlobalOptions _options;
        private List<GinRepositoryData> _repositories;
        private GinApplicationContext _parent;

        private GinRepositoryData _selectedRepository;
        private bool _suppressEvents;

        private void RecreateClient()
        {
            _client = new GinServiceClient(new InstanceContext(_parent));
            _client.InnerChannel.OperationTimeout = TimeSpan.MaxValue;
            _client.InnerDuplexChannel.OperationTimeout = TimeSpan.MaxValue;
        }

        public RepoManagement(GinServiceClient client, GinApplicationContext.GlobalOptions options,
            GinApplicationContext.UserCredentials credentials, GinApplicationContext parent)
        {
            InitializeComponent();
            _options = options;
            _credentials = credentials;
            _parent = parent;
        }

        private void RepoManagement_Load(object sender, EventArgs e)
        {
            RecreateClient();
            _repositories =
                new List<GinRepositoryData>(
                    JsonConvert.DeserializeObject<GinRepositoryData[]>(_client.GetRepositoryList()));
            _client.Close();

            foreach (var repo in _repositories)
                lvwRepositories.Items.Add(repo.Name);
        }

        private void lvwRepositories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwRepositories.SelectedIndices.Count <= 0)
            {
                _selectedRepository = null;
                DisableControls();
                return;
            }

            _suppressEvents = true;
            var repoName = lvwRepositories.SelectedItems[0];
            _selectedRepository = _repositories.Single(r => String.CompareOrdinal(r.Name, repoName.Text) == 0);
            txtRepoName.Text = _selectedRepository.Name;
            txtGinCommandline.Text = _selectedRepository.Address;
            txtMountpoint.Text = _selectedRepository.Mountpoint.FullName;
            txtPhysdir.Text = _selectedRepository.PhysicalDirectory.FullName;
            _suppressEvents = false;
            EnableControls();
        }

        private void EnableControls()
        {
            foreach (var ctrl in Controls)
            {
                if (ctrl == txtMountpoint || ctrl == txtPhysdir || ctrl == txtRepoName) continue;

                ((Control) ctrl).Enabled = true;
            }
        }

        private void DisableControls()
        {
            foreach (var ctrl in Controls)
            {
                if (ctrl == lvwRepositories) continue;

                ((Control) ctrl).Enabled = false;
            }
        }

        private void RepoManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            RecreateClient();
            _client.UnmmountAllRepositories();
            _client.Close();

            if (_repositories.Count == 0) return;

            foreach (var repo in _repositories)
            {
                RecreateClient();
                _client.AddRepository(repo.PhysicalDirectory.FullName, repo.Mountpoint.FullName, repo.Name,
                    repo.Address,
                    _options.RepositoryCheckoutOption ==
                    GinApplicationContext.GlobalOptions.CheckoutOption.FullCheckout, repo.CreateNew);
                _client.Close();

                repo.CreateNew = false;
            }
            var saveFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                           @"\g-node\GinWindowsClient\SavedRepositories.json";

            if (!Directory.Exists(Path.GetDirectoryName(saveFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(saveFile));

            if (File.Exists(saveFile))
                File.Delete(saveFile);


            var fs = File.CreateText(saveFile);
            fs.Write(JsonConvert.SerializeObject(_repositories));
            fs.Flush();
            fs.Close();
        }

        private void txtRepoName_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtGinCommandline_TextChanged(object sender, EventArgs e)
        {
            if (_suppressEvents) return;
            if (_selectedRepository == null) return;

            _selectedRepository.Address = txtGinCommandline.Text;
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            var getcmdlinedlg = new GetGinCmdline(_credentials);
            var result = getcmdlinedlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                var repoAddress = getcmdlinedlg.RepositoryName;
                var repoName = repoAddress.Split('/')[1];
                var repoPhysAddress = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                      @"\g-node\GinWindowsClient\Repositories\" + repoName;
                var repoMountpoint = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) +
                                     @"\Gin Repositories\" + repoName;

                var newRepo = new GinRepositoryData(new DirectoryInfo(repoPhysAddress),
                    new DirectoryInfo(repoMountpoint), repoName, repoAddress, getcmdlinedlg.CreateNew);

                _repositories.Add(newRepo);

                lvwRepositories.Items.Clear();
                foreach (var repo in _repositories)
                    lvwRepositories.Items.Add(repo.Name);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (_selectedRepository == null) return;

            var res = MessageBox.Show(Resources.RepoManagement_Delete_repository,
                Resources.GinClientApp_Gin_Client_Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (res != DialogResult.Yes) return;

            RecreateClient();
            _client.DeleteRepository(_selectedRepository.Name);
            _client.Close();
            _repositories.Remove(_selectedRepository);
            lvwRepositories.Items.Clear();

            foreach (var repo in _repositories)
                lvwRepositories.Items.Add(repo.Name);
        }

        private string PickDirectory(string defaultDir)
        {
            var dirPick = new FolderBrowserDialog();
            dirPick.SelectedPath = defaultDir;
            dirPick.ShowNewFolderButton = true;

            return dirPick.ShowDialog() == DialogResult.OK ? dirPick.SelectedPath : defaultDir;
        }

        private void btnPickMountPnt_Click(object sender, EventArgs e)
        {
            var newDir = PickDirectory(txtMountpoint.Text);

            if (String.Compare(newDir, txtMountpoint.Text) == 0) return;

            txtMountpoint.Text = newDir;
        }

        private void btnPickPhysDir_Click(object sender, EventArgs e)
        {
            var newDir = PickDirectory(txtPhysdir.Text);

            if (String.Compare(newDir, txtPhysdir.Text) == 0) return;

            txtPhysdir.Text = newDir;
        }
    }
}