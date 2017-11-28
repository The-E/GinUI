using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GinClientApp.GinClientService;
using GinClientLibrary;

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

        public GinApplicationContext()
        {
            _trayIcon = new NotifyIcon
            {
                ContextMenu = new ContextMenu(new[]
                {
                    new MenuItem("Exit", Exit)
                }),
                Visible = true,
                Icon = new Icon("gin_icon.ico")
            };

            _trayIcon.DoubleClick += _trayIcon_DoubleClick;

            _client = new GinClientServiceClient(new System.ServiceModel.InstanceContext(this));
            _client.AddRepository(
                @"C:\Users\fwoltermann\Desktop\gin-cli-builds",
                @"C:\Users\fwoltermann\Desktop\ginui-test\Test\",
                "Test",
                ""
            );
        }

        void IGinClientServiceCallback.FileOperationFinished(string filename, string repository, bool success)
        {
            throw new NotImplementedException();
        }

        void IGinClientServiceCallback.FileOperationStarted(string filename, string repository)
        {
            throw new NotImplementedException();
        }


        private void _trayIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _trayIcon_BalloonTipShown(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Repo_FileOperationCompleted(object sender, DokanInterface.FileOperationEventArgs e)
        {
        }


        private void Repo_FileOperationStarted(object sender, DokanInterface.FileOperationEventArgs e)
        {
            var repo = (GinRepository) sender;

            _trayIcon.BalloonTipTitle = @"GIN Repository activity in progress";
            _trayIcon.BalloonTipText =
                $"Retrieving {Path.GetFileName(e.File)} from repository {repo.Name}";
            _trayIcon.ShowBalloonTip(5000);
        }

        private void _trayIcon_DoubleClick(object sender, EventArgs e)
        {
            Exit(this, null);
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