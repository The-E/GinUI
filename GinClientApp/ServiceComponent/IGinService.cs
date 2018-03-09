using System.Collections.Generic;
using System.ServiceModel;
using GinClientLibrary;

namespace GinService
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IGinService
    {
        /// <summary>
        ///     Adds a new repository to the list of managed Repositories. If necessary,
        ///     this will also create the directories for the repository and perform an
        ///     initial checkout.
        /// </summary>
        /// <param name="physicalDirectory">A full path to a directory with a gin repository (or where a new one can be created)</param>
        /// <param name="mountpoint">A full path to a directory where a mountpoint should be created</param>
        /// <param name="name">The name of this repository</param>
        /// <param name="commandline">The gin get commandline used to initialize this, e.g. "gin get username/repository"</param>
        /// <param name="performFullCheckout">When true, all files are checked out of the annex, e.g "gin get </param>
        /// <param name="createNew">When true, this repository will be created new, i.e. through gin create</param>
        /// <returns>True if repository creation succeeded</returns>
        [OperationContract]
        bool AddRepository(string physicalDirectory, string mountpoint, string name, string commandline,
            bool performFullCheckout, bool createNew);

        /// <summary>
        ///     Create a new repository on the remote server
        /// </summary>
        /// <param name="repoName"></param>
        /// <returns></returns>
        [OperationContract]
        bool CreateNewRepository(string repoName);

        /// <summary>
        ///     Unmounts a repository.
        /// </summary>
        /// <param name="repoName"></param>
        /// <returns></returns>
        [OperationContract]
        bool MountRepository(string repoName);

        [OperationContract]
        bool UnmountRepository(string repoName);

        /// <summary>
        ///     Completely deletes a repository and all data associated with it
        /// </summary>
        /// <param name="repoName"></param>
        [OperationContract]
        void DeleteRepository(string repoName);

        /// <summary>
        ///     Unmounts all currently managed repositories
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool UnmmountAllRepositories();

        /// <summary>
        ///     Logs a user into GIN
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [OperationContract]
        bool Login(string username, string password);

        [OperationContract(IsOneWay = true)]
        void Logout();

        /// <summary>
        ///     Get a list of all currently managed repositories
        /// </summary>
        /// <returns>A JSON representation of a GinRepositoryData array</returns>
        [OperationContract]
        string GetRepositoryList();

        /// <summary>
        ///     Get the RepoData for the repo specified by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A JSON representation of a GinRepositoryData structure</returns>
        [OperationContract]
        string GetRepositoryInfo(string name);

        /// <summary>
        ///     Get repository info from the remote server
        /// </summary>
        /// <param name="path">The full path to the gin repository (i.e. user/repo)</param>
        /// <returns></returns>
        [OperationContract]
        string GetRemoteRepositoryInfo(string path);

        /// <summary>
        ///     Updates the stored information for the Repository indicated by repoName
        /// </summary>
        /// <param name="repoName">The previous name of the repository</param>
        /// <param name="data">The new data</param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateRepository(string repoName, GinRepositoryData data);

        /// <summary>
        ///     Performs a gin get-content operation on a file
        /// </summary>
        /// <param name="repoName">The repository in which the file resides</param>
        /// <param name="filepath">The path to the file</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = true)]
        void RetrieveFile(string repoName, string filepath);

        /// <summary>
        ///     Upload a file to the remote GIN repository
        /// </summary>
        /// <param name="repoName">Name of the repository</param>
        /// <param name="filepath">Path to the file</param>
        /// <returns></returns>
        [OperationContract]
        void UploadFile(string repoName, string filepath);

        /// <summary>
        ///     Updates the repository to the current version on the remote server.
        ///     Equivalent to gin download.
        /// </summary>
        /// <param name="repoName">Name of the repository</param>
        [OperationContract]
        void DownloadUpdateInfo(string repoName);

        /// <summary>
        ///     Updates all repositories
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void DownloadAllUpdateInfo();

        /// <summary>
        ///     Get a JSON representation of the current status of every file in the repository
        /// </summary>
        /// <param name="repoName">Name of the Repository</param>
        /// <returns>A JSON representation of a Dictionary&lt;string, GinRepository.FileStatus&gt;</returns>
        [OperationContract]
        string GetRepositoryFileInfo(string repoName);

        /// <summary>
        ///     Return a string with the gin status of a given file
        /// </summary>
        /// <param name="path">Fully qualified path to the file</param>
        /// <returns></returns>
        [OperationContract]
        string GetFileInfo(string path);

        /// <summary>
        ///     Check whether a given path is part of any managed repository
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [OperationContract(IsInitiating = true)]
        bool IsManagedPath(string filePath);

        /// <summary>
        ///     As IsManagedPath, but does not terminate the WCF instance
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [OperationContract(IsInitiating = true)]
        bool IsManagedPathNonTerminating(string filePath);

        /// <summary>
        ///     Check whether a given path is a repository base path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [OperationContract(IsInitiating = true)]
        bool IsBasePath(string filePath);

        /// <summary>
        ///     Performs a gin download on all specified repositories
        /// </summary>
        /// <param name="filePaths"></param>
        [OperationContract(IsOneWay = true)]
        void UpdateRepositories(IEnumerable<string> filePaths);

        /// <summary>
        ///     Upload all non-synced files in the specified Repos
        /// </summary>
        /// <param name="filePaths"></param>
        [OperationContract(IsOneWay = true)]
        void UploadRepositories(IEnumerable<string> filePaths);

        /// <summary>
        ///     Retrieves the specified files from their repositories
        /// </summary>
        /// <param name="filePaths"></param>
        [OperationContract(IsOneWay = true)]
        void DownloadFiles(IEnumerable<string> filePaths);

        /// <summary>
        ///     Remove all local content of the files indicated
        /// </summary>
        /// <param name="filePaths"></param>
        [OperationContract(IsOneWay = true)]
        void RemoveLocalContent(IEnumerable<string> filePaths);

        /// <summary>
        ///     Return the output of gin --version
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GetGinCliVersion();

        /// <summary>
        ///     Returns a JSON representation of a Dictionary(string, string),
        ///     listing all available Repositories for the current user
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GetRemoteRepositoryList();

        /// <summary>
        ///     Sets the GIN_CONFIG_DIR and GIN_LOG_DIR environment variables for the service
        /// </summary>
        /// <param name="AppDataPath"></param>
        /// <param name="LocalAppDataPath"></param>
        [OperationContract(IsOneWay = true)]
        void SetEnvironmentVariables(string AppDataPath, string LocalAppDataPath);

        /// <summary>
        ///     Ends the session, logs out the current user and unmounts all currently mounted repos
        /// </summary>
        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void EndSession();

        /// <summary>
        ///     Check if the service is alive
        /// </summary>
        /// <returns>true if it is, wcf error otherwise</returns>
        [OperationContract]
        bool IsAlive();
    }
}