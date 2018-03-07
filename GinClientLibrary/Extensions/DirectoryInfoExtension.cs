using System;
using System.IO;
using System.Linq;

namespace GinClientLibrary.Extensions
{
    public static class DirectoryInfoExtension
    {
        public static void Empty(this DirectoryInfo directory)
        {
            try
            {
                File.SetAttributes(directory.FullName,
                    File.GetAttributes(directory.FullName) & ~(FileAttributes.Hidden | FileAttributes.ReadOnly));

                foreach (var file in directory.GetFiles("*", SearchOption.AllDirectories))
                    try
                    {
                        File.SetAttributes(file.FullName, FileAttributes.Normal);
                        file.Delete();
                    }
                    catch
                    {
                    }

                foreach (var subDirectory in directory.GetDirectories())
                    try
                    {
                        File.SetAttributes(subDirectory.FullName, FileAttributes.Normal);
                        subDirectory.Delete(true);
                    }
                    catch
                    {
                    }
            }
            catch
            {
            }
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