using System.IO;
using System.Runtime.Serialization;

namespace GinClientLibrary
{
    [DataContract]
    public class GinRepositoryData
    {
        public GinRepositoryData(DirectoryInfo physicalDirectory, DirectoryInfo mountPoint, string repoName, string repoAddress, bool createNew)
        {
            PhysicalDirectory = physicalDirectory;
            Mountpoint = mountPoint;
            Name = repoName;
            Address = repoAddress;
            CreateNew = createNew;
        }

        /// <summary>
        ///     Name of the Repository, i.e. "Experiment data"
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Path to a directory containing the actual files
        /// </summary>
        [DataMember]
        public DirectoryInfo PhysicalDirectory { get; set; }

        /// <summary>
        ///     Path where the Repo will be mounted
        /// </summary>
        [DataMember]
        public DirectoryInfo Mountpoint { get; set; }

        /// <summary>
        ///     The gin commandline address used for checkouts, i.e. "achilleas/gin-cli-builds"
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        ///     Whether or not this repository is currently mounted
        /// </summary>
        [DataMember]
        public bool Mounted { get; set; }

        /// <summary>
        ///     Whether or not this repository is being created from scratch
        /// </summary>
        [DataMember]
        public bool CreateNew { get; set; }
    }
}
