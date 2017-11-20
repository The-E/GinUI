using DokanNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using Newtonsoft.Json;

namespace GinClient
{
    public class GinRepository
    {
        public string URL { get; set; }
        public string Name { get; set; }

        public enum FileStatus
        {
            InAnnex,
            OnDisk
        }

        #region Annex return values
        struct AnnexFileInfo
        {
            public string key;
            public bool present;
            public string file;
            public string command;
            public string size;
            public bool success;
        }

        struct AnnexGet
        {
            public string key;
            public string file;
            public string command;
            public string note;
            public bool success;
        }
        #endregion

        public FileStatus GetFileStatus(string filePath)
        {

            var cli = new Cli("cmd.exe");
            var cmd = "gin annex info " + filePath + " --json";
            var output = cli.Execute(cmd);

            AnnexFileInfo fileInfo = JsonConvert.DeserializeObject<AnnexFileInfo>(output.ToString());

            return fileInfo.present ? FileStatus.OnDisk : FileStatus.InAnnex;
        }

        public bool RetrieveFile(string filePath)
        {
            var cli = new Cli("cmd.exe");
            var cmd = "gin annex get " + filePath + " --json";
            var output = cli.Execute(cmd);

            AnnexGet result = JsonConvert.DeserializeObject<AnnexGet>(output.ToString());

            return result.success;
        }
    }

}
