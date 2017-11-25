using DokanNet;
using GinClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            {
                var thread = new Thread(repo.Mount);
                thread.Start();

                _repothreads.Add(repo, thread);
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

        public void AddRepository(DirectoryInfo physicalDirectory, DirectoryInfo mountpoint, string name, string url, string username, string password)
        {
            var _repo = new GinRepository(
                physicalDirectory,
                mountpoint,
                name,
                url,
                username,
                password);

            _repo.FileOperationStarted += Repo_FileOperationStarted;
            _repo.FileOperationCompleted += Repo_FileOperationCompleted;
            _repo.Initialize();

            Repositories.Add(_repo);
        }

        private void Repo_FileOperationCompleted(object sender, DokanInterface.FileOperationEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Repo_FileOperationStarted(object sender, DokanInterface.FileOperationEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
