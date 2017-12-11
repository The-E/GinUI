using System;
using System.IO;
using System.Linq;

namespace GinClientLibrary.Extensions
{
    internal static class DirectoryInfoExtension
    {
        public static void Empty(this DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
        
        public static bool IsEmpty(this DirectoryInfo directory)
        {
            if (!Directory.Exists(directory.FullName))
                return true;

            return !Directory.EnumerateFileSystemEntries(directory.FullName).Any();
        }

        public static bool IsEqualTo(this DirectoryInfo left, DirectoryInfo right)
        {
            return string.Equals(left.FullName, right.FullName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
