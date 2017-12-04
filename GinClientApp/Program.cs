using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.Windows.Controls;
using System.Windows.Forms;
using GinClientApp.GinClientService;
using GinClientLibrary;
using Newtonsoft.Json;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace GinClientApp
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GinApplicationContext());
        }
    }


    public class GinApplicationContext : ApplicationContext, IGinClientServiceCallback
    {
        private readonly GinClientServiceClient _client;
        private readonly NotifyIcon _trayIcon;

        private ProgressDisplay progressDisplay;

        public GinApplicationContext()
        {
            var saveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                               @"\gnode\GinWindowsClient\SavedRepositories.json";

            if (File.Exists(saveFilePath))
            {
                try
                {
                    var text = File.OpenText(saveFilePath).ReadToEnd();
                    var repos = JsonConvert.DeserializeObject<GinRepository[]>(text);

                    foreach (var repo in repos)
                    {
                        _client.AddRepository(repo.PhysicalDirectory.FullName, repo.Mountpoint.FullName, repo.Name, repo.Commandline);
                    }
                }
                catch (Exception e)
                {

                }
            }

            _trayIcon = new NotifyIcon
            {
                ContextMenu = new ContextMenu(BuildContextMenu()),
                Visible = true,
                Icon = new Icon("gin_icon.ico")
            };

            _trayIcon.DoubleClick += _trayIcon_DoubleClick;

            _client = new GinClientServiceClient(new InstanceContext(this));
            _client.AddRepository(
                @"C:\Users\fwoltermann\Desktop\gin-cli-builds",
                @"C:\Users\fwoltermann\Desktop\ginui-test\Test\",
                "Test",
                ""
            );
        }

        private MenuItem[] BuildContextMenu()
        {
            var menuitems = new List<MenuItem>();

            var repositories = _client.GetRepositoryList();

            foreach (var repo in repositories)
            {
                var mitem = new MenuItem(repo.Name);
                mitem.Tag = repo;
                mitem.MenuItems.Add("Edit", EditRepoMenuItemHandler);
                mitem.MenuItems.Add("Unmount", UnmountRepoMenuItemHandler);

                menuitems.Add(mitem);
            }

            if (repositories.Length != 0)
                menuitems.Add(new MenuItem("-"));

            menuitems.Add(new MenuItem("Manage Repositories", ManageRepositoriesMenuItemHandler));

            menuitems.Add(new MenuItem("Exit", Exit));

            return menuitems.ToArray();
        }

        private void ManageRepositoriesMenuItemHandler(object sender, EventArgs e)
        {
            var repomanager = new RepoManagement(_client);
            repomanager.Closed += (o, args) => { _trayIcon.ContextMenu = new ContextMenu(BuildContextMenu()); };
            repomanager.Show();
        }

        private void UnmountRepoMenuItemHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EditRepoMenuItemHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void IGinClientServiceCallback.FileOperationFinished(string filename, string repository, bool success)
        {
            //progressDisplay.RemoveFileTransfer(filename);
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

            //if (progressDisplay != null)
            //{
            //    progressDisplay.SetProgressBarState(filename, state, progress, speed);
            //}
        }

        void IGinClientServiceCallback.GinServiceError(string message)
        {
            MessageBox.Show(message, "GIN Service Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}