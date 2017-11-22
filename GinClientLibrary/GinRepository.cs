using DokanNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            OnDisk,
            Unknown
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

        private static StringBuilder _output = new StringBuilder("");

        public FileStatus GetFileStatus(string filePath)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = Directory.GetParent(filePath).FullName;
            startInfo.Arguments = "/c gin annex info " + filePath + " --json";
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.OutputDataReceived += Process_OutputDataReceived;
            _output.Clear();
            process.Start();
            process.BeginOutputReadLine();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            var output = _output.ToString();
            try
            {
                if (!string.IsNullOrEmpty(output))
                {
                    AnnexFileInfo fileInfo = JsonConvert.DeserializeObject<AnnexFileInfo>(output);

                    return fileInfo.present ? FileStatus.OnDisk : FileStatus.InAnnex;
                }

                return FileStatus.Unknown;
            }
            catch
            {
                return FileStatus.Unknown;
            }
        }

        private void Process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _output.Append(e.Data);
            }
        }

        public bool RetrieveFile(string filePath)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = Directory.GetParent(filePath).FullName;
            var filename = Path.GetFileName(filePath);
            startInfo.Arguments = "/C gin get-content " + filename + "--json";
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            process.StartInfo = startInfo;
            process.OutputDataReceived += Process_OutputDataReceived;
            _output.Clear();
            process.Start();
            process.BeginOutputReadLine();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            var output = _output.ToString();
            _output.Clear();

            try
            {
                if (string.IsNullOrEmpty(output))
                    return false;

                AnnexGet result = JsonConvert.DeserializeObject<AnnexGet>(output.ToString());

                return result.success;
            }
            catch
            {
                return false;
            }
        }

        public bool Login()
        {
            //if you wanna do the POST request in the Windows client separately, you can just 
            //POST to /api/v1/users/$USERNAME/tokens with data {"name":"gin-cli"} and header 
            //"content-type: application/json" and "Authorization: Basic <base64 encoded $USERNAME:$PASSWORD>"
            //default host gin.g-node.org
            //request returns a token that needs to be saved and attached to future requests
            //default path %userprofile%\.config\gin\, will be changed to %appdata%\gnode\gin\

            return true;
        }
    }

}
