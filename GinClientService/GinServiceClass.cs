﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using GinClientLibrary;
using Newtonsoft.Json;

namespace GinService
{
    public class GinServiceClass : IGinService
    {
        public GinServiceClass()
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
        

        bool IGinService.AddRepository(string physicalDirectory, string mountpoint, string name, string commandline, bool performFullCheckout, bool createNew)
        {
            RepositoryManager.Instance.AddRepository(new DirectoryInfo(physicalDirectory),
                new DirectoryInfo(mountpoint), name, commandline, performFullCheckout, createNew);
            return true;
        }

        bool IGinService.CreateNewRepository(string repoName)
        {
            return RepositoryManager.Instance.CreateNewRepository(repoName);
        }

        void IGinService.DownloadAllUpdateInfo()
        {
            foreach (var repo in RepositoryManager.Instance.Repositories)
            {
                repo.DownloadUpdateInfo();
            }
        }

        void IGinService.DownloadUpdateInfo(string repoName)
        {
            var repo = RepositoryManager.Instance.GetRepoByName(repoName);

            repo.DownloadUpdateInfo();
        }

        string IGinService.GetRepositoryList()
        {
            var result = RepositoryManager.Instance.Repositories.Select(repo => repo as GinRepositoryData).ToArray();
            return JsonConvert.SerializeObject(result);
        }

        bool IGinService.Login(string username, string password)
        {
            return RepositoryManager.Instance.Login(username, password);
        }

        bool IGinService.RetrieveFile(string repoName, string filepath)
        {
            var repo = RepositoryManager.Instance.GetRepoByName(repoName);

            return repo.RetrieveFile(filepath);
        }

        bool IGinService.StashFile(string repoName, string filepath)
        {
            var repo = RepositoryManager.Instance.GetRepoByName(repoName);

            return repo.RemoveFile(filepath);
        }

        bool IGinService.UploadFile(string repoName, string filepath)
        {
            var repo = RepositoryManager.Instance.GetRepoByName(repoName);

            return repo.UploadFile(filepath);
        }

        bool IGinService.UnmmountAllRepositories()
        {
            RepositoryManager.Instance.UnmountAllRepositories();
            return true;
        }

        bool IGinService.UnmountRepository(string repoName)
        {
            RepositoryManager.Instance.UnmountRepository(
                RepositoryManager.Instance.GetRepoByName(repoName));
            return true;
        }

        void IGinService.DeleteRepository(string repoName)
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

        string IGinService.GetRepositoryFileInfo(string repoName)
        {
            return RepositoryManager.Instance.GetRepositoryFileInfo(RepositoryManager.Instance.GetRepoByName(repoName));
        }

        bool IGinService.UpdateRepository(string repoName, GinRepositoryData data)
        {
            return RepositoryManager.Instance.UpdateRepository(repoName, data);
        }

        bool IGinService.MountRepository(string repoName)
        {
            RepositoryManager.Instance.MountRepository(RepositoryManager.Instance.GetRepoByName(repoName));
            return true;
        }

        bool IGinService.IsManagedPath(string filePath)
        {
            return RepositoryManager.Instance.IsManagedPath(filePath);
        }

        bool IGinService.IsBaseDirectory(string filePath)
        {
            return RepositoryManager.Instance.IsBaseDirectory(filePath);
        }
    }
}