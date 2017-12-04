using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace GinClientLibrary
{
    /// <summary>
    ///     Manages all repositories. Responsible for mounting and unmounting.
    /// </summary>
    public class RepositoryManager
    {
        public delegate void FileOperationProgressHandler(string filename, GinRepository repository, int progress,
            string speed, string state);

        public delegate void
            FileRetrievalCompletedHandler(object sender, GinRepository repo, string file, bool success);

        public delegate void FileRetrievalStartedHandler(object sender, GinRepository repo, string file);

        public delegate void RepositoryOperationErrorHandler(object sender,
            GinRepository.FileOperationErrorEventArgs message);

        private static RepositoryManager _instance;

        //private readonly Dictionary<GinRepository, Thread> _repothreads = new Dictionary<GinRepository, Thread>();

        private readonly List<GinServer> _servers = new List<GinServer>();

        private List<GinRepository> _repositories;

        private RepositoryManager()
        {
        }

        public static RepositoryManager Instance => _instance ?? (_instance = new RepositoryManager());

        public List<GinRepository> Repositories => _repositories ?? (_repositories = new List<GinRepository>());

        public GinRepository GetRepoByName(string name)
        {
            return Repositories.Single(r => string.Compare(r.Name, name, true) == 0);
        }

        public bool AddCredentials(string url, string username, string password)
        {
            var serverExists = false;

            foreach (var server in _servers)
            {
                if (serverExists)
                    continue;
                serverExists = string.Compare(server.URL, url, true) == 0;

                if (serverExists)
                {
                    var serv = _servers[_servers.IndexOf(server)];
                    serv.URL = url;
                    serv.Password = password;
                    serv.Username = username;
                }
            }

            if (!serverExists)
            {
                var newServer = new GinServer {URL = url, Username = username, Password = password};
                _servers.Add(newServer);
            }

            return true;
        }

        public string GetPasswordForUrl(string url)
        {
            throw new NotImplementedException();
        }

        public string GetUserNameForUrl(string url)
        {
            throw new NotImplementedException();
        }

        public void MountAllRepositories()
        {
            foreach (var repo in Repositories)
                MountRepository(repo);
        }

        private void MountRepository(GinRepository repo)
        {
            //if (!_repothreads.ContainsKey(repo))
            //{
            var thread = new Thread(repo.Mount);
            thread.Start();

            //_repothreads.Add(repo, thread);
            //}
        }

        public bool UpdateRepository(string repoName, GinRepository data)
        {
            lock (this)
            {
                var repo = Repositories.Single(r => string.Compare(r.Name, repoName) == 0);
                UnmountRepository(repo);
                Repositories.Remove(repo);
                Repositories.Add(data);
                MountRepository(data);

                return true;
            }
        }

        public void UnmountRepository(GinRepository repo)
        {
            repo.Dispose();
            //_repothreads[repo].Abort();
        }

        public void UnmountAllRepositories()
        {
            foreach (var repo in Repositories)
                UnmountRepository(repo);

            Repositories.Clear();
        }

        public void AddRepository(DirectoryInfo physicalDirectory, DirectoryInfo mountpoint, string name, string url)
        {
            var _repo = new GinRepository(
                physicalDirectory,
                mountpoint,
                name,
                url);

            _repo.FileOperationStarted += Repo_FileOperationStarted;
            _repo.FileOperationCompleted += Repo_FileOperationCompleted;
            _repo.FileOperationProgress += _repo_FileOperationProgress;
            _repo.FileOperationError += RepoOnFileOperationError;
            MountRepository(_repo);
            _repo.Initialize();


            Repositories.Add(_repo);
        }

        private void RepoOnFileOperationError(object sender,
            GinRepository.FileOperationErrorEventArgs fileOperationErrorEventArgs)
        {
            OnRepositoryOperationError((GinRepository) sender, fileOperationErrorEventArgs);
        }

        public event FileOperationProgressHandler FileOperationProgress;

        private void _repo_FileOperationProgress(object sender, string message)
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

        protected void OnRepositoryOperationError(GinRepository sender,
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

        private struct GinServer
        {
            public string URL;
            public string Username;
            public string Password;
        }
    }
}