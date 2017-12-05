using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using GinClientLibrary;

namespace GinClientService
{
    public class GinClientService : IGinClientService
    {
        public GinClientService()
        {
            RepositoryManager.Instance.MountAllRepositories();
            var callback = OperationContext.Current.GetCallbackChannel<IGinClientCallback>();

            RepositoryManager.Instance.FileRetrievalStarted +=
                (sender, repo, file) => callback.FileOperationStarted(file, repo.Name);
            RepositoryManager.Instance.FileRetrievalCompleted +=
                (sender, repo, file, success) => callback.FileOperationFinished(file, repo.Name, success);
            RepositoryManager.Instance.FileOperationProgress +=
                (filename, repo, progress, speed, state) =>
                    callback.FileOperationProgress(filename, repo.Name, progress, speed, state);
            RepositoryManager.Instance.RepositoryOperationError += (sender, message) =>
                callback.GinServiceError("Error while performing GIN action on Repository " + message.RepositoryName +
                                         ": " + message.Message);
        }
        

        bool IGinClientService.AddRepository(string physicalDirectory, string mountpoint, string name, string commandline)
        {
            RepositoryManager.Instance.AddRepository(new DirectoryInfo(physicalDirectory),
                new DirectoryInfo(mountpoint), name, commandline);
            return true;
        }

        void IGinClientService.DownloadUpdateInfo(string repoName)
        {
            var repo = RepositoryManager.Instance.GetRepoByName(repoName);

            repo.DownloadUpdateInfo();
        }

        List<GinRepositoryData> IGinClientService.GetRepositoryList()
        {
            return RepositoryManager.Instance.Repositories.Select(repo => repo as GinRepositoryData).ToList();
        }

        bool IGinClientService.Login(string username, string password)
        {
            return RepositoryManager.Instance.Login(username, password);
        }

        bool IGinClientService.RetrieveFile(string repoName, string filepath)
        {
            var repo = RepositoryManager.Instance.GetRepoByName(repoName);

            return repo.RetrieveFile(filepath);
        }

        bool IGinClientService.StashFile(string repoName, string filepath)
        {
            var repo = RepositoryManager.Instance.GetRepoByName(repoName);

            return repo.RemoveFile(filepath);
        }

        bool IGinClientService.UnmmountAllRepositories()
        {
            RepositoryManager.Instance.UnmountAllRepositories();
            return true;
        }

        bool IGinClientService.UnmountRepository(string repoName)
        {
            RepositoryManager.Instance.UnmountRepository(
                RepositoryManager.Instance.GetRepoByName(repoName));
            return true;
        }

        void IGinClientService.DeleteRepository(string repoName)
        {
            try
            {
                var repo = RepositoryManager.Instance.GetRepoByName(repoName);
                RepositoryManager.Instance.DeleteRepository(repo);
            }
            catch (Exception e)
            {
            }
        }


        bool IGinClientService.UpdateRepository(string repoName, GinRepositoryData data)
        {
            return RepositoryManager.Instance.UpdateRepository(repoName, data);
        }
    }
}