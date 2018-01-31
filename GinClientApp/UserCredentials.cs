using System;
using System.IO;
using Newtonsoft.Json;

namespace GinClientApp
{
    /// <summary>
    ///     A singleton class representing the current user's login credentials
    /// </summary>
    public class UserCredentials : ICloneable
    {
        private static UserCredentials _instance;

        private UserCredentials()
        {
        }

        public static UserCredentials Instance
        {
            get => _instance ?? (_instance = new UserCredentials());
            set => _instance = value;
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public object Clone()
        {
            return new UserCredentials {Username = Username, Password = Password};
        }

        public static bool Load()
        {
            var saveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                               @"\g-node\GinWindowsClient\UserCredentials.json";

            if (!Directory.Exists(Path.GetDirectoryName(saveFilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(saveFilePath));

            if (!File.Exists(saveFilePath)) return false;

            try
            {
                using (var freader = File.OpenText(saveFilePath))
                {
                    var text = freader.ReadToEnd();
                    _instance = JsonConvert.DeserializeObject<UserCredentials>(text);
                }

                return true;
            }
            catch
            {
                _instance = new UserCredentials();
                return false;
            }
        }

        public static void Save()
        {
            var saveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                               @"\g-node\GinWindowsClient\UserCredentials.json";

            if (File.Exists(saveFilePath))
                File.Delete(saveFilePath);

            using (var fwriter = File.CreateText(saveFilePath))
            {
                fwriter.Write(JsonConvert.SerializeObject(_instance));
            }
        }
    }
}