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
        public DirectoryInfo PhysicalDirectory { get; set; }
        public DirectoryInfo Mountpoint { get; set; }

        public enum FileStatus
        {
            InAnnex,
            InAnnexModified,
            OnDisk,
            OnDiskModified,
            Unknown,
            Directory
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
        
        public Dictionary<string, FileStatus> StatusCache {
            get
            {
                if (_scache == null)
                    _scache = new Dictionary<string, FileStatus>();
                return _scache;
            }
        }

        private Dictionary<string, FileStatus> _scache;

        /// <summary>
        /// Retrieve the status of every file in this repository
        /// Possible statuses are: 
        /// -In Annex
        /// -On Disk
        /// -On Disk, modified
        /// -Unknown (this includes files not yet added to the gin working tree)
        /// </summary>
        public void ReadRepoStatus()
        {
            string error = "";
            var output = GetCommandLineOutput("cmd.exe", "/c gin ls", PhysicalDirectory.FullName, out error);

            //The output is currently human-readable plaintext, need to parse that.

            var lines = output.Split(new char[] { '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            FileStatus status = FileStatus.Unknown;
            foreach (var line in lines)
            {
                if (line.CompareTo("Synced:") == 0)
                {
                    status = FileStatus.OnDisk;
                    continue;
                }
                else if (line.CompareTo("No local content:") == 0) {
                    status = FileStatus.InAnnex;
                    continue;
                }
                else if (line.CompareTo("Locally modified (unsaved):") == 0) {
                    status = FileStatus.OnDiskModified;
                    continue;
                }
                else if (line.CompareTo("Locally modified (not uploaded):") == 0) {
                    status = FileStatus.OnDiskModified;
                    continue;
                }
                else if (line.CompareTo("Remotely modified (not downloaded):") == 0) {
                    status = FileStatus.InAnnexModified;
                    continue;
                }
                else if (line.CompareTo("Unlocked for editing:") == 0) {
                    status = FileStatus.OnDisk;
                    continue;
                }
                else if (line.CompareTo("Removed:") == 0) {
                    status = FileStatus.Unknown;
                    continue;
                }
                else if (line.CompareTo("Untracked:") == 0) {
                    status = FileStatus.Unknown;
                    continue;
                }
                else if (line.CompareTo("Unknown:") == 0) {
                    status = FileStatus.Unknown;
                    continue;
                }

                var fullPath = Path.GetFullPath(PhysicalDirectory.FullName + Path.DirectorySeparatorChar + line);

                if (!StatusCache.ContainsKey(fullPath))
                    StatusCache.Add(fullPath, status);
                else
                    StatusCache[fullPath] = status;
            }
        }

        public FileStatus GetFileStatus(string filePath)
        {
            if (StatusCache.ContainsKey(filePath))
            {
                return StatusCache[filePath];
            }

            if (Directory.Exists(filePath))
                return FileStatus.Directory;


            //Windows will sometimes try to inspect the contents of a zip file; we need to catch this here and return the filestatus of the zip
            string parentDirectory = Directory.GetParent(filePath).FullName;
            if (parentDirectory.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
            {
                return GetFileStatus(parentDirectory);
            }

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

            ReadRepoStatus();

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
                _output.AppendLine(e.Data);
            }
        }

        private Object thisLock = new Object();
        private string GetCommandLineOutput(string program, string commandline, string workingDirectory, out string error)
        {
            lock (thisLock) {
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
        }
        #endregion
    }

}
