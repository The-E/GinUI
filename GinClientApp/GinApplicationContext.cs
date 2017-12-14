using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using GinClientApp.Dialogs;
using GinClientApp.GinService;
using GinClientApp.Properties;
using GinClientLibrary;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace GinClientApp
{
    public class GinApplicationContext : ApplicationContext, IGinServiceCallback
    {
        private GinServiceClient _client;
        private readonly NotifyIcon _trayIcon;
        private readonly UserCredentials _credentials;
        private GlobalOptions _options;
        private Timer _updateIntervalTimer;

        public class GlobalOptions
        {
            public int RepositoryUpdateInterval { get; set; }
            public CheckoutOption RepositoryCheckoutOption { get; set; }

            public enum CheckoutOption
            {
                AnnexCheckout,
                FullCheckout
            }

            public GlobalOptions()
            {
                RepositoryUpdateInterval = 15;
                RepositoryCheckoutOption = CheckoutOption.AnnexCheckout;
            }
        }

        private ProgressDisplay progressDisplay;

        private void RecreateClient()
        {
            _client = new GinServiceClient(new InstanceContext(this));
            _client.InnerDuplexChannel.Faulted += InnerChannelOnFaulted;
            _client.InnerChannel.OperationTimeout = TimeSpan.MaxValue;
            _client.InnerDuplexChannel.OperationTimeout = TimeSpan.MaxValue;
        }

        public GinApplicationContext()
        {

            var saveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                               @"\g-node\GinWindowsClient";
            if (!Directory.Exists(saveFilePath))
                Directory.CreateDirectory(saveFilePath);

            #region Read options

            if (File.Exists(saveFilePath + @"\GlobalOptionsDlg.json"))
            {
                try
                {
                    var text = File.OpenText(saveFilePath + @"\GlobalOptionsDlg.json").ReadToEnd();
                    _options = JsonConvert.DeserializeObject<GlobalOptions>(text);
                }
                catch
                {
                    var optionsDlg = new GlobalOptionsDlg(new GlobalOptions());
                    var res = optionsDlg.ShowDialog();

                    _options = res == DialogResult.OK ? optionsDlg.Options : new GlobalOptions();
                }
            }
            else
            {
                var optionsDlg = new GlobalOptionsDlg(new GlobalOptions());
                var res = optionsDlg.ShowDialog();

                _options = res == DialogResult.OK ? optionsDlg.Options : new GlobalOptions();

                var fs = File.CreateText(saveFilePath + @"\GlobalOptionsDlg.json");
                fs.Write(JsonConvert.SerializeObject(_options));
                fs.Flush();
                fs.Close();
            }

            if (_options.RepositoryUpdateInterval > 0)
            {
                _updateIntervalTimer = new Timer(_options.RepositoryUpdateInterval * 1000) {AutoReset = true};
                _updateIntervalTimer.Elapsed += (sender, args) =>
                {
                    RecreateClient(); _client.DownloadAllUpdateInfo(); _client.Close(); };
            }

            #endregion

            #region Login
            bool loggedIn = false;
            if (File.Exists(saveFilePath + @"\Credentials.json"))
            {
                try
                {
                    var text = File.OpenText(saveFilePath + @"\Credentials.json").ReadToEnd();
                    _credentials = JsonConvert.DeserializeObject<UserCredentials>(text);

                    RecreateClient();
                    if (!_client.Login(_credentials.Username, _credentials.Password))
                    {
                        MessageBox.Show(Resources.GinApplicationContext_Error_while_trying_to_log_in_to_GIN, Resources.GinApplicationContext_Gin_Client_Error,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        loggedIn = true;
                    }
                    _client.Close();
                }
                catch (Exception e)
                {
                    loggedIn = false;
                }
            }
            else
            {
                var loginDlg = new GetUserCredentials(this);
                var loginResult = loginDlg.ShowDialog();

                if (loginResult == DialogResult.OK)
                {
                    var credentials =
                        new UserCredentials() {Username = loginDlg.Username, Password = loginDlg.Password };

                    var fstream = File.CreateText(saveFilePath + @"\Credentials.json");
                    fstream.Write(JsonConvert.SerializeObject(credentials));
                    fstream.Flush();
                    fstream.Close();

                    loggedIn = true;
                }
            }

            if (!loggedIn)
                Exit(null, new EventArgs());
            #endregion

            #region Set up repositories

            if (File.Exists(saveFilePath + @"\SavedRepositories.json"))
            {
                try
                {
                    var text = File.OpenText(saveFilePath + @"\SavedRepositories.json").ReadToEnd();
                    var repos = JsonConvert.DeserializeObject<GinRepositoryData[]>(text);

                    foreach (var repo in repos)
                    {
                        RecreateClient();
                        _client.AddRepository(repo.PhysicalDirectory.FullName, repo.Mountpoint.FullName, repo.Name,
                            repo.Address, _options.RepositoryCheckoutOption == GlobalOptions.CheckoutOption.FullCheckout, false);
                        _client.Close();
                    }
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                ManageRepositoriesMenuItemHandler(null, EventArgs.Empty);
            }

            #endregion

            _trayIcon = new NotifyIcon
            {
                ContextMenu = new ContextMenu(BuildContextMenu()),
                Visible = true,
                Icon = new Icon("gin_icon.ico")
            };

            _trayIcon.DoubleClick += _trayIcon_DoubleClick;
            _updateIntervalTimer?.Start();
        }

        private void InnerChannelOnFaulted(object sender1, EventArgs eventArgs)
        {
            MessageBox.Show(Resources.GinApplicationContext_Gin_Service_has_stopped_communicating_,
                Resources.GinApplicationContext_Gin_Service_Error, MessageBoxButtons.OK);
            Exit(null, EventArgs.Empty);
        }

        private MenuItem[] BuildContextMenu()
        {
            var menuitems = new List<MenuItem>();

            RecreateClient();
            var repositories = JsonConvert.DeserializeObject<GinRepositoryData[]>(_client.GetRepositoryList());
            _client.Close();
            foreach (var repo in repositories)
            {
                var mitem = new MenuItem(repo.Name) {Tag = repo};
                //mitem.MenuItems.Add("Edit", EditRepoMenuItemHandler);
                mitem.MenuItems.Add(Resources.GinApplicationContext_Upload, UploadRepoMenuItemHandler);
                mitem.MenuItems.Add(Resources.GinApplicationContext_Unmount, UnmountRepoMenuItemHandler);
                mitem.MenuItems.Add(Resources.GinApplicationContext_Update, UpdateRepoMenuItemHandler);

                menuitems.Add(mitem);
            }

            if (repositories.Length != 0)
                menuitems.Add(new MenuItem("-"));

            menuitems.Add(new MenuItem(Resources.GinApplicationContext_Manage_Repositories, ManageRepositoriesMenuItemHandler));

            menuitems.Add(new MenuItem(Resources.GinApplicationContext_Options, ShowOptionsMenuItemHandler));

            menuitems.Add(new MenuItem(Resources.GinApplicationContext_Exit, Exit));

            return menuitems.ToArray();
        }

        private void UploadRepoMenuItemHandler(object sender, EventArgs e)
        {
            var repo = (GinRepositoryData)((MenuItem)sender).Parent.Tag;
            RecreateClient();
            var fstatus = JsonConvert.DeserializeObject <
                          Dictionary<string, GinRepository.FileStatus>>(_client.GetRepositoryFileInfo(repo.Name));
            _client.Close();

            var alteredFiles = from kvp in fstatus
                where kvp.Value == GinRepository.FileStatus.OnDiskModified ||
                      kvp.Value == GinRepository.FileStatus.Unknown
                select kvp;

            var files = alteredFiles as KeyValuePair<string, GinRepository.FileStatus>[] ?? alteredFiles.ToArray();
            if (!files.Any())
                return; //Nothing to upload here

            var uploadfiledlg = new UploadFilesDlg(files);
            var res = uploadfiledlg.ShowDialog();

            if (res == DialogResult.Cancel) return;

            foreach (var file in files)
            {
                RecreateClient();
                _client.UploadFile(repo.Name, file.Key);
                _client.Close();
            }
        }

        private void ShowOptionsMenuItemHandler(object sender, EventArgs e)
        {
            var optionsDlg = new GlobalOptionsDlg(_options);
            var res = optionsDlg.ShowDialog();

            if (res == DialogResult.OK)
            {
                _options = optionsDlg.Options;
                if (_options.RepositoryUpdateInterval <= 0) return;

                if (_updateIntervalTimer == null)
                {
                    _updateIntervalTimer = new Timer(_options.RepositoryUpdateInterval * 1000) { AutoReset = true };
                    _updateIntervalTimer.Elapsed += (sender1, args) => { RecreateClient(); _client.DownloadAllUpdateInfo(); _client.Close();};
                }
                _updateIntervalTimer.Stop();
                _updateIntervalTimer.Interval = _options.RepositoryUpdateInterval * 1000;
                _updateIntervalTimer.Start();
            }
        }

        private void UpdateRepoMenuItemHandler(object sender, EventArgs e)
        {
            var repo = (GinRepositoryData)((MenuItem) sender).Parent.Tag;

            RecreateClient();
            _client.DownloadUpdateInfo(repo.Name);
            _client.Close();
        }

        private void ManageRepositoriesMenuItemHandler(object sender, EventArgs e)
        {
            var repomanager = new RepoManagement(_options, _credentials, this);
            repomanager.Closed += (o, args) => { if (_trayIcon!= null) _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu()); };
            repomanager.ShowDialog();

            RecreateClient();
            _client.UnmmountAllRepositories();
            _client.Close();

            if (repomanager.Repositories.Count == 0) return;

            foreach (var repo in repomanager.Repositories)
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
            fs.Write(JsonConvert.SerializeObject(repomanager.Repositories));
            fs.Flush();
            fs.Close();
        }

        private void UnmountRepoMenuItemHandler(object sender, EventArgs e)
        {
            
            var mItem = (MenuItem) sender;
            var repo = (GinRepositoryData)mItem.Parent.Tag;
            RecreateClient();
            if (string.CompareOrdinal(Resources.GinApplicationContext_Unmount, mItem.Text) == 0)
            {
                _client.UnmountRepository(repo.Name);

                mItem.Text = Resources.GinApplicationContext_Mount;
            }
            else
            {
                _client.MountRepository(repo.Name);
                mItem.Text = Resources.GinApplicationContext_Unmount;
            }
            _client.Close();
        }

        private void EditRepoMenuItemHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void IGinServiceCallback.FileOperationFinished(string filename, string repository, bool success)
        {
            //progressDisplay?.RemoveFileTransfer(filename);
        }

        void IGinServiceCallback.FileOperationStarted(string filename, string repository)
        {
            _trayIcon.BalloonTipTitle = Resources.GinApplicationContext_Repository_Activity;
            _trayIcon.BalloonTipText =
                string.Format(Resources.GinApplicationContext_FileOperation_Retrieving, Path.GetFileName(filename), repository);
            _trayIcon.ShowBalloonTip(5000);

            //if (progressDisplay == null)
            //    progressDisplay = new ProgressDisplay();

            //progressDisplay.AddFileTransfer(filename);
            //progressDisplay.Show();
        }

        void IGinServiceCallback.FileOperationProgress(string filename, string repository, int progress,
            string speed, string state)
        {
            Console.WriteLine("Filename: {0}, Repo: {1}, Progress: {2}, Speed: {3}, State: {4}", filename, repository,
                progress, speed, state);

            //progressDisplay?.SetProgressBarState(filename, state, progress, speed);
        }

        void IGinServiceCallback.GinServiceError(string message)
        {
            MessageBox.Show(message, Resources.GinApplicationContext_Gin_Service_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Exit(this, EventArgs.Empty);
        }

        private void _trayIcon_DoubleClick(object sender, EventArgs e)
        {
            //TODO: Implement a management interface
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            if (_trayIcon != null)
                _trayIcon.Visible = false;

            if (_client != null && _client.InnerChannel.State != CommunicationState.Faulted)
            {
                RecreateClient();
                _client.UnmmountAllRepositories();
                _client.Close();
            }

            Application.Exit();
        }

        public struct UserCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}