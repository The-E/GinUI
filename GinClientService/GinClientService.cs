using System.Collections.Generic;
using System.Linq;
using GinClientLibrary;
using System.IO;

namespace GinClientService
{
    public class GinClientService : IGinClientService
    {
        public GinClientService() => RepositoryManager.Instance.MountAllRepositories();

        bool IGinClientService.AddCredentials(string url, string username, string password)
        {
            return RepositoryManager.Instance.AddCredentials(url, username, password);
        }

        bool IGinClientService.AddRepository(string physicalDirectory, string mountpoint, string name, string url)
        {
            RepositoryManager.Instance.AddRepository(new DirectoryInfo(physicalDirectory), new DirectoryInfo(mountpoint), name, url, RepositoryManager.Instance.GetUserNameForUrl(url), RepositoryManager.Instance.GetPasswordForUrl(url));
            return true;
        }

        List<GinRepository> IGinClientService.GetRepositoryList()
        {
            return RepositoryManager.Instance.Repositories;
        }

        bool IGinClientService.UnmmountAllRepositories()
        {
            RepositoryManager.Instance.UnmountAllRepositories();
            return true;
        }

        bool IGinClientService.UnmountRepository(string repoName)
        {
            RepositoryManager.Instance.UnmountRepository(RepositoryManager.Instance.Repositories.Single(r => string.Compare(r.Name, repoName, true) == 0));
            return true;
        }

        bool IGinClientService.UpdateRepository(string repoName, GinRepository data)
        {
            return RepositoryManager.Instance.UpdateRepository(repoName, data);
        }
    }
}
