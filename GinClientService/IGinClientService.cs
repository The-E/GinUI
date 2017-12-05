using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using GinClientLibrary;

namespace GinClientService
{
    [ServiceContract(CallbackContract = typeof(IGinClientCallback))]
    public interface IGinClientService
    {
        [OperationContract]
        bool AddRepository(string physicalDirectory, string mountpoint, string name, string commandline);

        [OperationContract]
        bool UnmountRepository(string repoName);

        [OperationContract]
        void DeleteRepository(string repoName);

        [OperationContract]
        bool UnmmountAllRepositories();

        [OperationContract]
        bool Login(string username, string password);

        [OperationContract]
        List<GinRepository> GetRepositoryList();

        [OperationContract]
        bool UpdateRepository(string repoName, GinRepository data);

        [OperationContract]
        bool RetrieveFile(string repoName, string filepath);

        [OperationContract]
        bool StashFile(string repoName, string filepath);

        [OperationContract]
        void DownloadUpdateInfo(string repoName);
    }

    [SuppressMessage("ReSharper", "OperationContractWithoutServiceContract")]
    public interface IGinClientCallback
    {
        [OperationContract]
        void FileOperationStarted(string filename, string repository);

        [OperationContract]
        void FileOperationFinished(string filename, string repository, bool success);

        [OperationContract]
        void FileOperationProgress(string filename, string repository, int progress, string speed, string state);

        [OperationContract]
        void GinServiceError(string message);
    }
}