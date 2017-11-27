using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GinClientLibrary;

namespace GinClientService
{
    [ServiceContract]
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

    //    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    //    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "GinClientService.ContractType".
    //    [DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
