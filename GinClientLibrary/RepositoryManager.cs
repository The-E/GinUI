using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DokanNet;
using Newtonsoft.Json;

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
        private static readonly StringBuilder _output = new StringBuilder("");
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                _output.AppendLine(e.Data);
        }

        private RepositoryManager()
        {
        }

        public static RepositoryManager Instance => _instance ?? (_instance = new RepositoryManager());

        public List<GinRepository> Repositories => _repositories ?? (_repositories = new List<GinRepository>());

        public GinRepository GetRepoByName(string name)
        {
            return Repositories.Single(r => string.Compare(r.Name, name, true) == 0);
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
            //TODO: Coordinate with achilleas to get the gin client to accept a --json parameter to gin repos
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
                        Arguments = "/C gin.exe create " + repoName,
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
                _output.Clear();
                process.Start();
                process.BeginOutputReadLine();
                process.StandardInput.WriteLine(password);
                var error = process.StandardError.ReadToEnd();
                
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    RepositoryManager.Instance.OnRepositoryOperationError(null, new GinRepository.FileOperationErrorEventArgs() { RepositoryName = "RepositoryManager", Message = error });
                    return false;
                }
            }

            return true;
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
                var repo = Repositories.Single(r => string.Compare(r.Name, repoName) == 0);
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
            UnmountRepository(repo);
            repo.DeleteRepository();
        }

        public void UnmountAllRepositories()
        {
            foreach (var repo in Repositories)
                UnmountRepository(repo);

            Repositories.Clear();
        }

        public void AddRepository(DirectoryInfo physicalDirectory, DirectoryInfo mountpoint, string name, string commandline, bool performFullCheckout, bool createNew)
        {
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

        protected void OnFileRetrievalStarted(DokanInterface.FileOperationEventArgs e, GinRepository sender)
        {
            FileRetrievalStarted?.Invoke(this, sender, e.File);
        }

        public event FileRetrievalCompletedHandler FileRetrievalCompleted;

        protected void OnFileRetrievalCompleted(DokanInterface.FileOperationEventArgs e, GinRepository sender)
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

        public void OnRepositoryOperationError(GinRepository sender,
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
                if (!string.IsNullOrEmpty(progress))
                    return int.Parse(progress.Trim('%'));

                return 0;
            }
        }
    }
}