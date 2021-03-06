﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using GinClientLibrary;
using Newtonsoft.Json;

namespace GinService
{
    /// <summary>
    ///     Main implementation of IGinService. This maps the functionality described in that interface on the
    ///     RepositoryManager functionality.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public class GinService : IGinService
    {
        public GinService()
        {
            var callback = OperationContext.Current.GetCallbackChannel<IGinClientCallback>();

            RepositoryManager.Instance.FileRetrievalStarted +=
                (sender, repo, file) =>
                {
                    try
                    {
                        callback.FileOperationStarted(file, repo.Name);
                    }
                    catch
                    {
                    }
                };
            RepositoryManager.Instance.FileRetrievalCompleted +=
                (sender, repo, file, success) =>
                {
                    try
                    {
                        callback.FileOperationFinished(file, repo.Name, success);
                    }
                    catch
                    {
                    }
                };
            RepositoryManager.Instance.FileOperationProgress +=
                (filename, repo, progress, speed, state) =>
                {
                    try
                    {
                        callback.FileOperationProgress(filename, repo.Name, progress, speed, state);
                    }
                    catch
                    {
                    }
                };
            RepositoryManager.Instance.RepositoryOperationError += (sender, message) =>
            {
                try
                {
                    callback.GinServiceError("Error while performing GIN action on Repository " +
                                             message.RepositoryName +
                                             ": " + message.Message);
                }
                catch
                {
                }
            };

            //We need to issue a logout at this point to clear any potentially invalid tokens
        }


        bool IGinService.AddRepository(string physicalDirectory, string mountpoint, string name, string commandline,
            bool performFullCheckout, bool createNew)
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
            lock (this)
            {
                foreach (var repo in RepositoryManager.Instance.Repositories)
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

        string IGinService.GetRepositoryInfo(string name)
        {
            var result = RepositoryManager.Instance.GetRepoByName(name) as GinRepositoryData;
            return JsonConvert.SerializeObject(result);
        }

        bool IGinService.Login(string username, string password)
        {
            return RepositoryManager.Instance.Login(username, password);
        }


        void IGinService.Logout()
        {
            RepositoryManager.Instance.Logout();
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
            catch
            {
            }
        }

        string IGinService.GetRepositoryFileInfo(string repoName)
        {
            return RepositoryManager.Instance.GetRepositoryFileInfo(RepositoryManager.Instance.GetRepoByName(repoName));
        }

        string IGinService.GetFileInfo(string path)
        {
            var repo = RepositoryManager.Instance.GetRepoByPath(path);
            return repo.GetFileStatus(path).ToString();
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

        bool IGinService.IsManagedPathNonTerminating(string filePath)
        {
            return RepositoryManager.Instance.IsManagedPath(filePath);
        }

        bool IGinService.IsBasePath(string filePath)
        {
            return RepositoryManager.Instance.IsBasePath(filePath);
        }

        void IGinService.UpdateRepositories(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                var repo = RepositoryManager.Instance.GetRepoByPath(filePath);

                repo?.DownloadUpdateInfo();
            }
        }

        void IGinService.UploadRepositories(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                var repo = RepositoryManager.Instance.GetRepoByPath(filePath);

                repo?.UploadRepository();
            }
        }

        void IGinService.DownloadFiles(IEnumerable<string> filePaths)
        {
            var files = filePaths as string[] ?? filePaths.ToArray();
            var repo = RepositoryManager.Instance.GetRepoByPath(files.First());

            foreach (var file in files)
                repo.RetrieveFile(file);
        }

        string IGinService.GetGinCliVersion()
        {
            return RepositoryManager.Instance.GetGinCliVersion();
        }

        string IGinService.GetRemoteRepositoryList()
        {
            return RepositoryManager.Instance.GetRemoteRepoList();
        }

        void IGinService.SetEnvironmentVariables(string AppDataPath, string LocalAppDataPath)
        {
            Environment.SetEnvironmentVariable("GIN_CONFIG_DIR", AppDataPath, EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable("GIN_LOG_DIR", LocalAppDataPath, EnvironmentVariableTarget.Machine);
        }

        void IGinService.EndSession()
        {
            RepositoryManager.Instance.UnmountAllRepositories();
            RepositoryManager.Instance.Logout();
        }
    }
}