using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;
using GinClientApp.Dialogs;
using GinClientApp.GinClientService;
using GinClientLibrary;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace GinClientApp
{
    public class GinApplicationContext : ApplicationContext, IGinClientServiceCallback
    {
        private readonly GinClientServiceClient _client;
        private readonly NotifyIcon _trayIcon;
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

        public GinApplicationContext()
        {
            try
            {
                _client = new GinClientServiceClient(new InstanceContext(this));

                if (_client.InnerChannel.State == CommunicationState.Faulted)
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Error while trying to access Gin Client Service", "Gin Client Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var saveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                               @"\gnode\GinWindowsClient";
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

                    if (res == DialogResult.OK)
                        _options = optionsDlg.Options;
                    else
                        _options = new GlobalOptions();
                }
            }
            else
            {
                var optionsDlg = new GlobalOptionsDlg(new GlobalOptions());
                var res = optionsDlg.ShowDialog();

                if (res == DialogResult.OK)
                    _options = optionsDlg.Options;
                else
                    _options = new GlobalOptions();

                var fs = File.CreateText(saveFilePath + @"\GlobalOptionsDlg.json");
                fs.Write(JsonConvert.SerializeObject(_options));
                fs.Flush();
                fs.Close();
            }

            _updateIntervalTimer = new Timer(_options.RepositoryUpdateInterval * 1000);
            _updateIntervalTimer.AutoReset = true;
            _updateIntervalTimer.Elapsed += (sender, args) => { _client.DownloadUpdateInfo(); };
            _updateIntervalTimer.Start();
            #endregion

            #region Login
            bool loggedIn = false;
            if (File.Exists(saveFilePath + @"\Credentials.json"))
            {
                try
                {
                    var text = File.OpenText(saveFilePath + @"\Credentials.json").ReadToEnd();
                    var credentials = JsonConvert.DeserializeObject<UserCredentials>(text);

                    if (!_client.Login(credentials.Username, credentials.Password))
                    {
                        MessageBox.Show("Error while trying to log in to GIN", "Gin Client Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        loggedIn = true;
                    }
                }
                catch (Exception e)
                {
                    loggedIn = false;
                }
            }
            else
            {
                var loginDlg = new GetUserCredentials(_client);
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
                        _client.AddRepository(repo.PhysicalDirectory.FullName, repo.Mountpoint.FullName, repo.Name,
                            repo.Commandline);
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
        }

        private MenuItem[] BuildContextMenu()
        {
            var menuitems = new List<MenuItem>();

            var repositories = JsonConvert.DeserializeObject<GinRepositoryData[]>(_client.GetRepositoryList());

            foreach (var repo in repositories)
            {
                var mitem = new MenuItem(repo.Name);
                mitem.Tag = repo;
                //mitem.MenuItems.Add("Edit", EditRepoMenuItemHandler);
                mitem.MenuItems.Add("Unmount", UnmountRepoMenuItemHandler);
                mitem.MenuItems.Add("Update", UpdateRepoMenuItemHandler);

                menuitems.Add(mitem);
            }

            if (repositories.Length != 0)
                menuitems.Add(new MenuItem("-"));

            menuitems.Add(new MenuItem("Manage Repositories", ManageRepositoriesMenuItemHandler));

            menuitems.Add(new MenuItem("Options", ShowOptionsMenuItemHandler));

            menuitems.Add(new MenuItem("Exit", Exit));

            return menuitems.ToArray();
        }

        private void ShowOptionsMenuItemHandler(object sender, EventArgs e)
        {
            var optionsDlg = new GlobalOptionsDlg(_options);
            var res = optionsDlg.ShowDialog();

            if (res == DialogResult.OK)
            {
                _options = optionsDlg.Options;
                _updateIntervalTimer.Stop();
                _updateIntervalTimer.Interval = _options.RepositoryUpdateInterval * 1000;
                _updateIntervalTimer.Start();
            }
        }

        private void UpdateRepoMenuItemHandler(object sender, EventArgs e)
        {
            var repo = (GinRepositoryData)((MenuItem) sender).Parent.Tag;

            _client.DownloadUpdateInfo(repo.Name);
        }

        private void ManageRepositoriesMenuItemHandler(object sender, EventArgs e)
        {
            var repomanager = new RepoManagement(_client);
            repomanager.Closed += (o, args) => { _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu()); };
            repomanager.ShowDialog();
        }

        private void UnmountRepoMenuItemHandler(object sender, EventArgs e)
        {
            
            var mItem = (MenuItem) sender;
            var repo = (GinRepositoryData)mItem.Parent.Tag;
            if (string.CompareOrdinal("Unmount", mItem.Text) == 0)
            {
                _client.UnmountRepository(repo.Name);
                mItem.Text = "Mount";
            }
            else
            {
                _client.MountRepository(repo.Name);
                mItem.Text = "Unmount";
            }
        }

        private void EditRepoMenuItemHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void IGinClientServiceCallback.FileOperationFinished(string filename, string repository, bool success)
        {
            //progressDisplay?.RemoveFileTransfer(filename);
        }

        void IGinClientServiceCallback.FileOperationStarted(string filename, string repository)
        {
            _trayIcon.BalloonTipTitle = @"GIN Repository activity in progress";
            _trayIcon.BalloonTipText =
                $"Retrieving {Path.GetFileName(filename)} from repository {repository}";
            _trayIcon.ShowBalloonTip(5000);

            //if (progressDisplay == null)
            //    progressDisplay = new ProgressDisplay();

            //progressDisplay.AddFileTransfer(filename);
            //progressDisplay.Show();
        }

        void IGinClientServiceCallback.FileOperationProgress(string filename, string repository, int progress,
            string speed, string state)
        {
            Console.WriteLine("Filename: {0}, Repo: {1}, Progress: {2}, Speed: {3}, State: {4}", filename, repository,
                progress, speed, state);

            //progressDisplay?.SetProgressBarState(filename, state, progress, speed);
        }

        void IGinClientServiceCallback.GinServiceError(string message)
        {
            MessageBox.Show(message, "GIN Service Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Exit(this, EventArgs.Empty);
        }

        private void _trayIcon_DoubleClick(object sender, EventArgs e)
        {
            //TODO: Implement a management interface
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;
            _client.UnmmountAllRepositories();
            _client.Close();
            Application.Exit();
        }

        struct UserCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}