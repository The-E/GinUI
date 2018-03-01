using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using GinClientApp.Dialogs;
using GinClientApp.GinService;
using GinClientApp.Properties;
using GinClientLibrary;
using GinService;
using Microsoft.Win32;
using Newtonsoft.Json;
using IGinService = GinClientApp.GinService.IGinService;
using Timer = System.Timers.Timer;

namespace GinClientApp
{
    /// <summary>
    ///     The main application context for the GINUI client.
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class GinApplicationContext : ApplicationContext, IGinServiceCallback
    {
        private readonly NotifyIcon _trayIcon;
        public GinServiceClient ServiceClient;
        private Timer _updateIntervalTimer;
        private Thread _serviceThread;

        public GinApplicationContext()
        {
            var service = new GinClientWindowsService();

            _serviceThread = new Thread(service.Start);
            _serviceThread.Start();

            SystemEvents.SessionEnded += SystemEvents_SessionEnded;

            _trayIcon = new NotifyIcon
            {
                Visible = true,
                Icon = Resources.gin_icon_desaturated
            };

            var myBinding = new WSDualHttpBinding
            {
                ClientBaseAddress = new Uri(@"http://localhost:8738/GinService/GinUI/" + Environment.UserName),
                MaxBufferPoolSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                OpenTimeout = TimeSpan.FromMinutes(1.0),
                CloseTimeout = TimeSpan.FromMinutes(1.0),
                SendTimeout = TimeSpan.FromHours(1),
                ReceiveTimeout = TimeSpan.FromHours(1),
                ReaderQuotas = new XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = int.MaxValue, MaxBytesPerRead = int.MaxValue, MaxDepth = int.MaxValue, MaxNameTableCharCount = int.MaxValue, MaxStringContentLength = int.MaxValue
                }
            };
            var endpointIdentity = EndpointIdentity.CreateDnsIdentity("localhost");
            var myEndpoint = new EndpointAddress(new Uri("http://localhost:8733/GinService/"), endpointIdentity);

            ServiceClient = new GinServiceClient(new InstanceContext(this), myBinding, myEndpoint);
            ServiceClient.InnerChannel.Faulted += InnerChannelOnFaulted;
            var saveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                @"\g-node\GinWindowsClient";
            if (!Directory.Exists(saveFilePath))
                Directory.CreateDirectory(saveFilePath);

            #region Environment Variables

            //Tell the service to use the current users' AppData folders for logging and config data
            ServiceClient.SetEnvironmentVariables(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\g-node\gin\",
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\g-node\gin\");

            #endregion

            #region Login

            if (!UserCredentials.Load())
            {
                var getUserCreds = new MetroGetUserCredentialsDlg(this);
                var result = getUserCreds.ShowDialog(); //The Dialog will log us in and save the user credentials

                if (result == DialogResult.Cancel)
                {
                    Exit(this, EventArgs.Empty);
                    return;
                }
            }
            else if (!ServiceClient.Login(UserCredentials.Instance.Username, UserCredentials.Instance.Password))
            {
                MessageBox.Show(Resources.GinApplicationContext_Error_while_trying_to_log_in_to_GIN,
                    Resources.GinApplicationContext_Gin_Client_Error,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                var getUserCreds = new MetroGetUserCredentialsDlg(this);
                var result = getUserCreds.ShowDialog(); //The Dialog will log us in and save the user credentials

                if (result == DialogResult.Cancel)
                {
                    Exit(this, EventArgs.Empty);
                    return;
                }
            }

            UserCredentials.Save();

            #endregion

            #region Read options

            if (!GlobalOptions.Load())
            {
                var optionsDlg = new MetroOptionsDlg(this, MetroOptionsDlg.Page.GlobalOptions);
                optionsDlg.RepoListingChanged += (o, args) => { _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu()); };
                var result = optionsDlg.ShowDialog();

                if (result == DialogResult.Cancel)
                    Exit(this, EventArgs.Empty);
            }

            if (GlobalOptions.Instance.RepositoryUpdateInterval > 0)
            {
                _updateIntervalTimer =
                    new Timer(GlobalOptions.Instance.RepositoryUpdateInterval * 1000 * 60) {AutoReset = true};
                _updateIntervalTimer.Elapsed += (sender, args) => { ServiceClient.DownloadAllUpdateInfo(); };
            }

            GlobalOptions.Save();

            #endregion

            #region Set up repositories

            if (File.Exists(saveFilePath + @"\SavedRepositories.json"))
                try
                {
                    using (var saveFile = File.OpenText(saveFilePath + @"\SavedRepositories.json"))
                    {
                        var text = saveFile.ReadToEnd();
                        var repos = JsonConvert.DeserializeObject<GinRepositoryData[]>(text);

                        foreach (var repo in repos)
                            ServiceClient.AddRepository(repo.PhysicalDirectory.FullName, repo.Mountpoint.FullName,
                                repo.Name,
                                repo.Address,
                                GlobalOptions.Instance.RepositoryCheckoutOption ==
                                GlobalOptions.CheckoutOption.FullCheckout, false);
                    }
                }
                catch
                {
                }
            else
                ManageRepositoriesMenuItemHandler(null, EventArgs.Empty);

            #endregion


            _trayIcon.DoubleClick += _trayIcon_DoubleClick;
            _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu());
            _trayIcon.Icon = Resources.gin_icon;
            _updateIntervalTimer?.Start();
            
        }

        private void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e)
        {
            SystemEvents.SessionEnded -= SystemEvents_SessionEnded;

            Exit(this, EventArgs.Empty);
        }

        void IGinServiceCallback.FileOperationFinished(string filename, string repository, bool success)
        {
        }

        void IGinServiceCallback.FileOperationStarted(string filename, string repository)
        {
            _trayIcon.BalloonTipTitle = Resources.GinApplicationContext_Repository_Activity;
            _trayIcon.BalloonTipText =
                string.Format(Resources.GinApplicationContext_FileOperation_Retrieving, Path.GetFileName(filename),
                    repository);
            _trayIcon.ShowBalloonTip(5000);
        }

        void IGinServiceCallback.FileOperationProgress(string filename, string repository, int progress,
            string speed, string state)
        {
            Console.WriteLine("Filename: {0}, Repo: {1}, Progress: {2}, Speed: {3}, State: {4}", filename, repository,
                progress, speed, state);
        }

        void IGinServiceCallback.GinServiceError(string message)
        {
            MessageBox.Show(message, Resources.GinApplicationContext_Gin_Service_Error, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            Exit(this, EventArgs.Empty);
        }

        private void InnerChannelOnFaulted(object sender1, EventArgs eventArgs)
        {
            ServiceClient.Abort();
            ServiceClient = null;

            var myBinding = new WSDualHttpBinding
            {
                ClientBaseAddress = new Uri(@"http://localhost:8738/GinService/GinUI/" + Environment.UserName),
                MaxBufferPoolSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                OpenTimeout = TimeSpan.FromMinutes(1.0),
                CloseTimeout = TimeSpan.FromMinutes(1.0),
                ReaderQuotas = new XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = int.MaxValue,
                    MaxBytesPerRead = int.MaxValue,
                    MaxDepth = int.MaxValue,
                    MaxNameTableCharCount = int.MaxValue,
                    MaxStringContentLength = int.MaxValue
                }
            };
            var endpointIdentity = EndpointIdentity.CreateDnsIdentity("localhost");
            var myEndpoint = new EndpointAddress(new Uri("http://localhost:8733/GinService/"), endpointIdentity);

            ServiceClient = new GinServiceClient(new InstanceContext(this), myBinding, myEndpoint);
        }

        private MenuItem[] BuildContextMenu()
        {
            var menuitems = new List<MenuItem>();

            var repositories = JsonConvert.DeserializeObject<GinRepositoryData[]>(ServiceClient.GetRepositoryList());
            foreach (var repo in repositories)
            {
                var mitem = new MenuItem(repo.Name) {Tag = repo};
                mitem.MenuItems.Add(Resources.GinApplicationContext_Upload, UploadRepoMenuItemHandler);
                mitem.MenuItems.Add(Resources.GinApplicationContext_Update, UpdateRepoMenuItemHandler);

                menuitems.Add(mitem);
            }

            if (repositories.Length != 0)
                menuitems.Add(new MenuItem("-"));

            menuitems.Add(new MenuItem(Resources.GinApplicationContext_Manage_Repositories,
                ManageRepositoriesMenuItemHandler));
            menuitems.Add(new MenuItem(Resources.GinApplicationContext_Options, ShowOptionsMenuItemHandler));
            menuitems.Add(new MenuItem(Resources.GinApplicationContext_About, ShowAboutMenuItemHandler));
            menuitems.Add(new MenuItem("-"));
            menuitems.Add(new MenuItem(Resources.GinApplicationContext_Exit, Exit));

            return menuitems.ToArray();
        }

        private void ShowAboutMenuItemHandler(object sender, EventArgs e)
        {
            var optionsdlg = new MetroOptionsDlg(this, MetroOptionsDlg.Page.About);
            optionsdlg.RepoListingChanged += (o, args) => { _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu()); };
            optionsdlg.Closed += (o, args) =>
            {
                if (_trayIcon != null) _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu());
            };
            optionsdlg.Show();
        }

        private void UploadRepoMenuItemHandler(object sender, EventArgs e)
        {
            var repo = (GinRepositoryData) ((MenuItem) sender).Parent.Tag;
            var fstatus = JsonConvert.DeserializeObject<
                Dictionary<string, GinRepository.FileStatus>>(ServiceClient.GetRepositoryFileInfo(repo.Name));

            var alteredFiles = from kvp in fstatus
                where kvp.Value == GinRepository.FileStatus.OnDiskModified ||
                      kvp.Value == GinRepository.FileStatus.Unknown || kvp.Value == GinRepository.FileStatus.Removed
                select kvp;

            var files = alteredFiles as KeyValuePair<string, GinRepository.FileStatus>[] ?? alteredFiles.ToArray();
            if (!files.Any())
                return; //Nothing to upload here

            var uploadfiledlg = new MetroUploadFilesDlg(files);
            var res = uploadfiledlg.ShowDialog();

            if (res == DialogResult.Cancel) return;

            //WCF requires that non-optional arguments be non-empty, so we provide a placeholder value
            ServiceClient.UploadFile(repo.Name, "%EMPTYSTRING%");
        }

        private void ShowOptionsMenuItemHandler(object sender, EventArgs e)
        {
            var optionsDlg = new MetroOptionsDlg(this, MetroOptionsDlg.Page.GlobalOptions);
            optionsDlg.RepoListingChanged += (o, args) => { _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu()); };
            optionsDlg.Closed += (o, args) =>
            {
                if (_trayIcon != null) _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu());
            };
            var res = optionsDlg.ShowDialog();

            if (res != DialogResult.OK) return;

            if (GlobalOptions.Instance.RepositoryUpdateInterval <= 0)
            {
                _updateIntervalTimer?.Stop();
                return;
            }

            if (_updateIntervalTimer == null)
            {
                _updateIntervalTimer =
                    new Timer(GlobalOptions.Instance.RepositoryUpdateInterval * 1000) {AutoReset = true};
                _updateIntervalTimer.Elapsed += (sender1, args) => { ServiceClient.DownloadAllUpdateInfo(); };
            }
            _updateIntervalTimer.Stop();
            _updateIntervalTimer.Interval = GlobalOptions.Instance.RepositoryUpdateInterval * 1000;
            _updateIntervalTimer.Start();
        }

        private void UpdateRepoMenuItemHandler(object sender, EventArgs e)
        {
            var repo = (GinRepositoryData) ((MenuItem) sender).Parent.Tag;

            ServiceClient.DownloadUpdateInfo(repo.Name);
        }

        private void ManageRepositoriesMenuItemHandler(object sender, EventArgs e)
        {
            var repomanager = new MetroOptionsDlg(this, MetroOptionsDlg.Page.Repositories);
            repomanager.RepoListingChanged += (o, args) => { _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu()); };
            repomanager.Closed += (o, args) =>
            {
                if (_trayIcon != null) _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu());
            };
            repomanager.ShowDialog();
        }

        private void _trayIcon_DoubleClick(object sender, EventArgs e)
        {
            var repomanager = new MetroOptionsDlg(this, MetroOptionsDlg.Page.Repositories);
            repomanager.RepoListingChanged += (o, args) => { _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu()); };
            repomanager.Closed += (o, args) =>
            {
                if (_trayIcon != null) _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu());
            };
            repomanager.ShowDialog();
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            if (_trayIcon != null)
                _trayIcon.Visible = false;

            if (ServiceClient != null && ServiceClient.InnerChannel.State != CommunicationState.Faulted)
                ServiceClient.EndSession();
            
            _serviceThread.Abort();
            _serviceThread.Join();
            Environment.Exit(0);
        }
    }
}