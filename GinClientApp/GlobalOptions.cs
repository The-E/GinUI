using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GinClientApp.Dialogs;
using Newtonsoft.Json;

namespace GinClientApp
{
    public class GlobalOptions
    {
        private static GlobalOptions _instance;

        public static GlobalOptions Instance => _instance ?? (_instance = new GlobalOptions());

        public int RepositoryUpdateInterval { get; set; }
        public CheckoutOption RepositoryCheckoutOption { get; set; }
        public DirectoryInfo DefaultCheckoutDir { get; set; }
        public DirectoryInfo DefaultMountpointDir { get; set; }


        public enum CheckoutOption
        {
            AnnexCheckout,
            FullCheckout
        }

        private GlobalOptions()
        {
            RepositoryUpdateInterval = 15;
            RepositoryCheckoutOption = CheckoutOption.AnnexCheckout;
            DefaultCheckoutDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\g-node\GinWindowsClient\Repositories");
            DefaultMountpointDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Gin Repositories");
        }

        public static void Save()
        {
            var saveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                               @"\g-node\GinWindowsClient\GlobalOptionsDlg.json";

            if (File.Exists(saveFilePath))
                File.Delete(saveFilePath);

            using (var fwriter = File.CreateText(saveFilePath))
            {
                fwriter.Write(JsonConvert.SerializeObject(_instance));
            }
        }

        public static bool Load()
        {
            var saveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                              @"\g-node\GinWindowsClient\GlobalOptionsDlg.json";
            if (!Directory.Exists(Path.GetDirectoryName(saveFilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(saveFilePath));

            if (!File.Exists(saveFilePath)) return false;

            try
            {
                using (var freader = File.OpenText(saveFilePath))
                {
                    var text = freader.ReadToEnd();
                    _instance = JsonConvert.DeserializeObject<GlobalOptions>(text);
                }

                return true;
            }
            catch
            {
                _instance = new GlobalOptions();
                return false;
            }
        }
    }
}
