using System;
using System.IO;
using Newtonsoft.Json;

namespace GinClientApp
{
    /// <summary>
    ///     A Singleton class representing the global options for the client.
    /// </summary>
    public class GlobalOptions : ICloneable
    {
        public enum CheckoutOption
        {
            AnnexCheckout,
            FullCheckout
        }

        private static GlobalOptions _instance;

        private GlobalOptions()
        {
            RepositoryUpdateInterval = 15;
            RepositoryCheckoutOption = CheckoutOption.AnnexCheckout;
            DefaultCheckoutDir =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                  @"\g-node\GinWindowsClient\Repositories");
            DefaultMountpointDir =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) +
                                  @"\Gin Repositories");
        }

        public static GlobalOptions Instance
        {
            get => _instance ?? (_instance = new GlobalOptions());
            set => _instance = value;
        }

        /// <summary>
        ///     The update interval, in minutes.
        /// </summary>
        public int RepositoryUpdateInterval { get; set; }

        /// <summary>
        ///     Default behaviour for the client when checking out a GIN repository
        /// </summary>
        public CheckoutOption RepositoryCheckoutOption { get; set; }

        /// <summary>
        ///     Default directory to put checkouts in.
        /// </summary>
        public DirectoryInfo DefaultCheckoutDir { get; set; }

        /// <summary>
        ///     Default directory for Mountpoints
        /// </summary>
        public DirectoryInfo DefaultMountpointDir { get; set; }

        public object Clone()
        {
            return new GlobalOptions
            {
                DefaultCheckoutDir = DefaultCheckoutDir,
                DefaultMountpointDir = DefaultMountpointDir,
                RepositoryUpdateInterval = RepositoryUpdateInterval,
                RepositoryCheckoutOption = RepositoryCheckoutOption
            };
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