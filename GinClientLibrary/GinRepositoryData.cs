using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GinClientLibrary
{
    [DataContract]
    public class GinRepositoryData
    {

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
        ///     The gin commandline used for checkouts, i.e. "gin get achilleas/gin-cli-builds"
        /// </summary>
        [DataMember]
        public string Commandline { get; set; }
    }
}
