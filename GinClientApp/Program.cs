using System;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;
using GinClientApp.GinClientService;

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

            _client = new GinClientServiceClient(new InstanceContext(this));
            _client.AddRepository(
                @"C:\Users\fwoltermann\Desktop\gin-cli-builds",
                @"C:\Users\fwoltermann\Desktop\ginui-test\Test\",
                "Test",
                ""
            );
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