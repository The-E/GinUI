using GinClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GinClientApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GinApplicationContext());
        }
    }

    public class GinApplicationContext : ApplicationContext
    {
        private NotifyIcon _trayIcon;
        private DokanInterface _dk;

        public GinApplicationContext()
        {
            _trayIcon = new NotifyIcon()
            {
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", Exit)
                }),
                Visible = true,
                Icon = new System.Drawing.Icon("gin_icon.ico")
            };

            _trayIcon.DoubleClick += _trayIcon_DoubleClick;


            GinRepository repo = new GinRepository(
                new DirectoryInfo(@"C:\Users\fwoltermann\Desktop\gin-cli-builds"), 
                new DirectoryInfo(@"C:\Users\fwoltermann\Desktop\ginui-test\Test\"), 
                "Test", 
                "", 
                "", 
                "");

            repo.FileOperationStarted += Repo_FileOperationStarted;
            repo.FileOperationCompleted += Repo_FileOperationCompleted;
            repo.Initialize();
            repo.Mount();            
        }

        private void Repo_FileOperationCompleted(object sender, DokanInterface.FileOperationEventArgs e)
        {
        }

        private void Repo_FileOperationStarted(object sender, DokanInterface.FileOperationEventArgs e)
        {
            using (var repo = (GinRepository)sender)
            {
                _trayIcon.BalloonTipTitle = "GIN Repository activity in progress";
                _trayIcon.BalloonTipText = "Retrieving " + Path.GetFileName(e.File) + " from repository " + repo.Name;
                _trayIcon.ShowBalloonTip(5000);
            }
        }

        private void _trayIcon_DoubleClick(object sender, EventArgs e)
        {
            
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;

            Application.Exit();
        }
    }
}
