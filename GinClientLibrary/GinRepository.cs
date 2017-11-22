using DokanNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
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
            string error;
            var output = GetCommandLineOutput("cmd.exe", "/c gin annex info " + filePath + " --json", Directory.GetParent(filePath).FullName, out error);
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

        public bool RetrieveFile(string filePath)
        {
            var directoryName = Directory.GetParent(filePath).FullName;
            string filename = Directory.GetFiles(directoryName).Single(s => string.Compare(s.ToUpperInvariant(), filePath.ToUpperInvariant()) == 0);
            filename = Path.GetFileName(filename);

            string error;
            var output = GetCommandLineOutput("cmd.exe", "/C gin get-content " + filename + " --json", directoryName, out error);
            _output.Clear();

            if (!string.IsNullOrEmpty(error))
                return false;
            else //gin currently returns an empty string on  success
                return true;
          
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

        #region Helpers
        private void Process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _output.Append(e.Data);
            }
        }

        private string GetCommandLineOutput(string program, string commandline, string workingDirectory, out string error)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = program,
                    WorkingDirectory = workingDirectory,
                    Arguments = commandline,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            process.OutputDataReceived += Process_OutputDataReceived;
            _output.Clear();
            process.Start();
            process.BeginOutputReadLine();
            error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            var output = _output.ToString();
            _output.Clear();
            return output;
        }
        #endregion
    }

}
