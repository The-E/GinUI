using System;
using System.IO;
using System.Reflection;
using GinClientApp.Dialogs;
using GinClientLibrary.Extensions;

namespace GinClientApp
{
    internal class Installer
    {
        public static void DoUninstall()
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup) + @"\GinClientApp.lnk"))
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup) + @"\GinClientApp.lnk");
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) +
                            @"\G-Node\GinClientApp.lnk"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) +
                            @"\G-Node\GinClientApp.lnk");
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) +
                                 @"\G-Node\");
            }

            var path = new DirectoryInfo(Assembly.GetCallingAssembly().Location).Parent;

            if (Directory.Exists(path.FullName + @"\gin-cli\"))
            {
                var dInfo = new DirectoryInfo(path.FullName + @"\gin-cli\");
                DirectoryInfoExtension.Empty(dInfo);
                Directory.Delete(path.FullName + @"\gin-cli\", true);
            }

            var deleteDlg = new DeleteDataDlg();

            deleteDlg.ShowDialog();

            var configDataPath = new DirectoryInfo(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                @"\g-node\GinWindowsClient\");

            if (!deleteDlg.KeepCheckout)
                if (Directory.Exists(configDataPath.FullName + @"\Repositories\"))
                {
                    var dInfo = new DirectoryInfo(configDataPath.FullName + @"\Repositories\");
                    DirectoryInfoExtension.Empty(dInfo);
                    Directory.Delete(dInfo.FullName, true);
                }

            if (!deleteDlg.KeepUserConfig)
                if (File.Exists(configDataPath.FullName + @"\GlobalOptionsDlg.json"))
                    File.Delete(configDataPath.FullName + @"\GlobalOptionsDlg.json");

            if (!deleteDlg.KeepUserLogin)
                if (File.Exists(configDataPath.FullName + @"\UserCredentials.json"))
                    File.Delete(configDataPath.FullName + @"\UserCredentials.json");
        }
    }
}