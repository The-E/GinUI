using DokanNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace GinClientLibrary
{
    /// <summary>
    /// Manages all repositories. Responsible for mounting and unmounting.
    /// </summary>
    public class RepositoryManager
    {
        private static RepositoryManager _instance;

        private RepositoryManager() { }

        public static RepositoryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RepositoryManager();
                }
                return _instance;
            }
        }

        private struct GinServer
        {
            public string URL;
            public string Username;
            public string Password;
        }

        private List<GinServer> _servers = new List<GinServer>();

        public bool AddCredentials(string url, string username, string password)
        {
            bool serverExists = false;

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
                var newServer = new GinServer() { URL = url, Username = username, Password = password };
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

        List<GinRepository> _repositories;
        public List<GinRepository> Repositories {
            get {
                if (_repositories == null)
                    _repositories = new List<GinRepository>();
                return _repositories;
            }
        }

        Dictionary<GinRepository, Thread> _repothreads = new Dictionary<GinRepository, Thread>();
        public void MountAllRepositories()
        {
            foreach (var repo in Repositories)
                MountRepository(repo);
        }

        void MountRepository(GinRepository repo)
        {
            if (!_repothreads.ContainsKey(repo))
            {
                var thread = new Thread(repo.Mount);
                thread.Start();

                _repothreads.Add(repo, thread);
            }
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
            _repothreads[repo].Abort();
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
            _repo.Initialize();

            MountRepository(_repo);
            Repositories.Add(_repo);
        }

        private void Repo_FileOperationCompleted(object sender, DokanInterface.FileOperationEventArgs e)
        {
            
        }

        private void Repo_FileOperationStarted(object sender, DokanInterface.FileOperationEventArgs e)
        {
            
        }
    }
}
