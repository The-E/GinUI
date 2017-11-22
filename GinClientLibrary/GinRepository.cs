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
        public string Username { get; set; }
        public string Password { get; set; }

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
            output.ThrowIfError();

            AnnexFileInfo fileInfo = JsonConvert.DeserializeObject<AnnexFileInfo>(output.ToString());

            return fileInfo.present ? FileStatus.OnDisk : FileStatus.InAnnex;
        }

        public bool RetrieveFile(string filePath)
        {
            var cli = new Cli("cmd.exe");
            var cmd = "gin annex get " + filePath + " --json";
            var output = cli.Execute(cmd);
            output.ThrowIfError();

            AnnexGet result = JsonConvert.DeserializeObject<AnnexGet>(output.ToString());

            return result.success;
        }

        public bool Login()
        {
            //if you wanna do the POST request in the Windows client separately, you can just 
            //POST to /api/v1/users/$USERNAME/tokens with data {"name":"gin-cli"} and header 
            //"content-type: application/json" and "Authorization: Basic <base64 encoded $USERNAME:$PASSWORD>"
            //default host gin.g-node.org
            //request returns a token that needs to be saved and attached to future requests

            return true;
        }
    }

}
