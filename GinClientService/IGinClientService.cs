using System.Collections.Generic;
using System.ServiceModel;
using GinClientLibrary;

namespace GinClientService
{
    [ServiceContract(CallbackContract = typeof(IGinClientCallback))]
    public interface IGinClientService
    {
        [OperationContract]
        bool AddRepository(string physicalDirectory, string mountpoint, string name, string url);

        [OperationContract]
        bool UnmountRepository(string repoName);

        [OperationContract]
        bool UnmmountAllRepositories();

        [OperationContract]
        bool AddCredentials(string url, string username, string password);

        [OperationContract]
        List<GinRepository> GetRepositoryList();

        [OperationContract]
        bool UpdateRepository(string repoName, GinRepository data);
    }

    public interface IGinClientCallback
    {
        [OperationContract]
        void FileOperationStarted(string filename, string repository);

        [OperationContract]
        void FileOperationFinished(string filename, string repository, bool success);
    }
}