using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DokanNet;
using GinClientLibrary.Extensions;
using Newtonsoft.Json;
using static System.String;

namespace GinClientLibrary
{
    /// <summary>
    ///     Manages all repositories. Responsible for mounting and unmounting.
    /// </summary>
    public class RepositoryManager
    {
        public delegate void FileOperationProgressHandler(string filename, GinRepositoryData repository, int progress,
            string speed, string state);

        public delegate void
            FileRetrievalCompletedHandler(object sender, GinRepositoryData repo, string file, bool success);

        public delegate void FileRetrievalStartedHandler(object sender, GinRepositoryData repo, string file);

        public delegate void RepositoryOperationErrorHandler(object sender,
            GinRepository.FileOperationErrorEventArgs message);

        private static RepositoryManager _instance;

        private List<GinRepository> _repositories;
        private static readonly StringBuilder Output = new StringBuilder("");
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!IsNullOrEmpty(e.Data))
                Output.AppendLine(e.Data);
        }

        private RepositoryManager()
        {
        }

        public static RepositoryManager Instance => _instance ?? (_instance = new RepositoryManager());

        public List<GinRepository> Repositories => _repositories ?? (_repositories = new List<GinRepository>());

        public GinRepository GetRepoByName(string name)
        {
            return Repositories.Single(r => Compare(r.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public void Logout()
        {
            lock (this)
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                        WorkingDirectory = @"C:\",
                        Arguments = "/C gin.exe logout",
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false
                    }
                };
                
                process.Start();
            }
        }

        public bool CreateNewRepository(string repoName)
        {
            //TODO: Return false if a repo with that name already exists

            lock (this)
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                        WorkingDirectory = @"C:\",
                        Arguments = "/C gin.exe create " + repoName + " --no-clone",
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false
                    }
                };

                process.Start();
            }

            return true;
        }

        public bool Login(string username, string password)
        {
            //if you wanna do the POST request in the Windows client separately, you can just 
            //POST to /api/v1/users/$USERNAME/tokens with data {"name":"gin-cli"} and header 
            //"content-type: application/json" and "Authorization: Basic <base64 encoded $USERNAME:$PASSWORD>"
            //default host gin.g-node.org
            //request returns a token that needs to be saved and attached to future requests
            //default path %userprofile%\.config\gin\, will be changed to %appdata%\g-node\gin\

            //Also note: In a service context, %userprofile% evaluates to C:\Windows\system32\config\systemprofile\; %AppData% to C:\Windows\system32\config\systemprofile\Appdata\Roaming

            lock (this)
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                        WorkingDirectory = @"C:\",
                        Arguments = @"/C gin.exe login " + username,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false
                    }
                };

                process.OutputDataReceived += Process_OutputDataReceived;
                Output.Clear();
                process.Start();
                process.BeginOutputReadLine();
                process.StandardInput.WriteLine(password);
                var error = process.StandardError.ReadToEnd();
                
                process.WaitForExit();

                if (IsNullOrEmpty(error)) return true;
                Instance.OnRepositoryOperationError(null, new GinRepository.FileOperationErrorEventArgs() { RepositoryName = "RepositoryManager", Message = error });
                return false;
            }
        }

        public GinRepository GetRepoByPath(string filePath)
        {
            return Repositories.Find(repo => filePath.Contains(repo.Mountpoint.FullName.Trim('\\')));
        }

        public bool IsBasePath(string filePath)
        {
            var attr = File.GetAttributes(filePath);
            if ((attr & FileAttributes.Directory) != FileAttributes.Directory) return false;
            var dInfo = new DirectoryInfo(filePath);

            return Repositories.Any(repo => repo.Mountpoint.IsEqualTo(dInfo));
        }

        public string GetGinCliVersion()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    WorkingDirectory = @"C:\",
                    Arguments = @"/C gin.exe --version",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false
                }
            };

            process.OutputDataReceived += Process_OutputDataReceived;
            Output.Clear();
            process.Start();
            process.BeginOutputReadLine();
            var error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            return Output.ToString();
        }

        public bool IsManagedPath(string filePath)
        {
            return Repositories.Any(repo => filePath.Contains(repo.Mountpoint.FullName));
        }

        public string GetRepositoryFileInfo(GinRepository ginRepository)
        {
            return ginRepository.GetStatusCacheJson();
        }

        public void MountAllRepositories()
        {
            foreach (var repo in Repositories)
                MountRepository(repo);
        }

        public void MountRepository(GinRepository repo)
        {
            if (!repo.Mounted)
            {
                var thread = new Thread(repo.Mount);
                thread.Start();
            }
        }

        public bool UpdateRepository(string repoName, GinRepositoryData data)
        {
            lock (this)
            {
                var repo = Repositories.Single(r => CompareOrdinal(r.Name, repoName) == 0);
                UnmountRepository(repo);
                Repositories.Remove(repo);
                repo = new GinRepository(data);
                Repositories.Add(repo);
                MountRepository(repo);

                return true;
            }
        }

        public void UnmountRepository(GinRepository repo)
        {
            Dokan.RemoveMountPoint(repo.Mountpoint.FullName.Trim('\\'));
            repo.Mounted = false;
        }

        public void DeleteRepository(GinRepository repo)
        {
            lock (this)
            {
                UnmountRepository(repo);
                repo.DeleteRepository();

                Repositories.Remove(repo);
            }
        }

        public void UnmountAllRepositories()
        {
            lock (this)
            {
                foreach (var repo in Repositories)
                    UnmountRepository(repo);

                Repositories.Clear();
            }
        }

        public void AddRepository(DirectoryInfo physicalDirectory, DirectoryInfo mountpoint, string name, string commandline, bool performFullCheckout, bool createNew)
        {
            //Repository already exists
            if (Repositories.Any(repository => string.Compare(repository.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0))
                return;

            var repo = new GinRepository(
                physicalDirectory,
                mountpoint,
                name,
                commandline, createNew);

            repo.FileOperationStarted += Repo_FileOperationStarted;
            repo.FileOperationCompleted += Repo_FileOperationCompleted;
            repo.FileOperationProgress += Repo_FileOperationProgress;
            repo.FileOperationError += RepoOnFileOperationError;
            repo.CreateDirectories(performFullCheckout);
            MountRepository(repo);
            repo.Initialize();

            Repositories.Add(repo);
        }

        private void RepoOnFileOperationError(object sender,
            GinRepository.FileOperationErrorEventArgs fileOperationErrorEventArgs)
        {
            OnRepositoryOperationError((GinRepository) sender, fileOperationErrorEventArgs);
        }

        public event FileOperationProgressHandler FileOperationProgress;

        private void Repo_FileOperationProgress(object sender, string message)
        {
            try
            {
                var progress = JsonConvert.DeserializeObject<fileOpProgress>(message);
                FileOperationProgress?.Invoke(progress.filename, (GinRepository) sender, progress.GetProgress(),
                    progress.rate, progress.state);
            }
            catch (Exception e)
            {
            }
        }

        public event FileRetrievalStartedHandler FileRetrievalStarted;

        private void OnFileRetrievalStarted(DokanInterface.FileOperationEventArgs e, GinRepository sender)
        {
            FileRetrievalStarted?.Invoke(this, sender, e.File);
        }

        public event FileRetrievalCompletedHandler FileRetrievalCompleted;

        private void OnFileRetrievalCompleted(DokanInterface.FileOperationEventArgs e, GinRepository sender)
        {
            FileRetrievalCompleted?.Invoke(this, sender, e.File, e.Success);
        }

        private void Repo_FileOperationCompleted(object sender, DokanInterface.FileOperationEventArgs e)
        {
            OnFileRetrievalCompleted(e, (GinRepository) sender);
        }

        private void Repo_FileOperationStarted(object sender, DokanInterface.FileOperationEventArgs e)
        {
            OnFileRetrievalStarted(e, (GinRepository) sender);
        }

        public event RepositoryOperationErrorHandler RepositoryOperationError;

        private void OnRepositoryOperationError(GinRepository sender,
            GinRepository.FileOperationErrorEventArgs message)
        {
            RepositoryOperationError?.Invoke(sender, message);
        }


        //{"filename":"gin-cli-0.12dev.deb","state":"Downloading","progress":"","rate":"","err":""}
        private struct fileOpProgress
        {
            public string filename { get; set; }
            public string state { get; set; }
            public string progress { get; set; }
            public string rate { get; set; }
            public string err { get; set; }

            public int GetProgress()
            {
                if (!IsNullOrEmpty(progress))
                    return int.Parse(progress.Trim('%'));

                return 0;
            }
        }

        public string GetRemoteRepoList()
        {
            return GetCommandLineOutput("cmd.exe", @"/C gin.exe repos --json --all", @"C:\", out string error);
        }

        private object _thisLock = new object();
        private string GetCommandLineOutput(string program, string commandline, string workingDirectory,
            out string error)
        {
            lock (_thisLock)
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
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
                Output.Clear();
                process.Start();
                process.BeginOutputReadLine();
                error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                var output = Output.ToString();
                Output.Clear();
                return output;
            }
        }
    }
}