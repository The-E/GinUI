﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using DokanNet;
using static DokanNet.FormatProviders;
using FileAccess = DokanNet.FileAccess;

namespace GinClientLibrary
{
    /// <summary>
    ///     Implements the IDokanOperations interface. See https://github.com/dokan-dev/dokan-dotnet for reference.
    ///     This code is largely based on the "Mirror" sample provided there.
    /// </summary>
    public class DokanInterface : IDokanOperations
    {
        #region GIN data

        /// <summary>
        ///     The GinRepository linked to this Dokan Interface. Provided mostly for convenience.
        /// </summary>
        private readonly GinRepository _repository;

        #endregion

        public static bool TestDokanAvailability()
        {
            return true;
        }

        #region Constructor/Destructor and setup

        /// <summary>
        ///     CreateDirectories a new Dokan Interface.
        /// </summary>
        /// <param name="repo"></param>
        /// The repository this is supporting
        /// <param name="enableLogging"></param>
        /// If set to true, log messages will be written to the Application event log
        public DokanInterface(GinRepository repo, bool enableLogging = true)
        {
            InitLogging();

            _repository = repo;
            _doLogging = enableLogging;
        }

        public class DokanInterfaceException : Exception
        {
            public DokanInterfaceException(Exception inner, string message) : base(message, inner)
            {
            }
        }

        /// <summary>
        ///     Mounts the repository and starts mirroring
        /// </summary>
        /// <exception cref="DokanInterfaceException">Thrown if the mount fails</exception>
        public void Initialize()
        {
            this.Mount(_repository.Mountpoint.FullName.Trim('\\'),
                DokanOptions.DebugMode | DokanOptions.StderrOutput);
        }

        #endregion

        #region Logging Infrastructure methods

        private readonly bool _doLogging;

        private NtStatus Trace(string method, string fileName, DokanFileInfo info, NtStatus result,
            params object[] parameters)
        {
            if (_doLogging)
            {
                var extraParameters = parameters != null && parameters.Length > 0
                    ? ", " + string.Join(", ", parameters.Select(x => string.Format(DefaultFormatProvider, "{0}", x)))
                    : string.Empty;
            }
            return result;
        }

        private NtStatus Trace(string method, string fileName, DokanFileInfo info,
            FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes,
            NtStatus result)
        {
            return result;
        }

        private void InitLogging()
        {
        }

        #endregion

        #region Events

        public class FileOperationEventArgs : EventArgs
        {
            public bool Success { get; set; }
            public string File { get; set; }
        }

        /// <summary>
        ///     Event is triggered everytime the driver retrieves a remote file
        /// </summary>
        public event FileOperationStartedHandler FileOperationStarted;

        public delegate void FileOperationStartedHandler(object sender, FileOperationEventArgs e);

        protected virtual void OnFileOperationStarted(FileOperationEventArgs e)
        {
            FileOperationStarted?.Invoke(this, e);
        }

        /// <summary>
        ///     Triggered once a retrieval operation completes
        /// </summary>
        public event FileOperationCompleteHandler FileOperationCompleted;

        public delegate void FileOperationCompleteHandler(object sender, FileOperationEventArgs e);

        protected virtual void OnFileOperationCompleted(FileOperationEventArgs e)
        {
            FileOperationCompleted?.Invoke(this, e);
        }

        #endregion

        #region Helpers

        /// <summary>
        ///     Translates a filename from the virtual filesystem to the actual one
        /// </summary>
        /// <param name="fileName">The filename, relative to the mountpoint</param>
        /// <returns></returns>
        private string GetPath(string fileName)
        {
            return _repository.PhysicalDirectory.FullName + fileName;
        }

        private IList<FileInformation> FindFilesHelper(string fileName, string searchPattern)
        {
            IList<FileInformation> files = new DirectoryInfo(GetPath(fileName))
                .EnumerateFileSystemInfos()
                .Where(finfo => DokanHelper.DokanIsNameInExpression(searchPattern, finfo.Name, true))
                .Select(finfo => new FileInformation
                {
                    Attributes = finfo.Attributes,
                    CreationTime = finfo.CreationTime,
                    LastAccessTime = finfo.LastAccessTime,
                    LastWriteTime = finfo.LastWriteTime,
                    Length = (finfo as FileInfo)?.Length ?? 0,
                    FileName = finfo.Name
                }).ToArray();

            return files;
        }

        #endregion

        #region Dokany interface implementation

        private const FileAccess DataAccess = FileAccess.ReadData | FileAccess.WriteData | FileAccess.AppendData |
                                              FileAccess.Execute |
                                              FileAccess.GenericExecute | FileAccess.GenericWrite |
                                              FileAccess.GenericRead;

        private const FileAccess DataWriteAccess = FileAccess.WriteData | FileAccess.AppendData |
                                                   FileAccess.Delete |
                                                   FileAccess.GenericWrite;

        public void Cleanup(string fileName, DokanFileInfo info)
        {
            if (info.Context != null && _doLogging)
                Console.WriteLine(DokanFormat($"{nameof(Cleanup)}('{fileName}', {info} - entering"));

            (info.Context as FileStream)?.Dispose();
            info.Context = null;

            if (info.DeleteOnClose)
                if (info.IsDirectory)
                    Directory.Delete(GetPath(fileName));
                else
                    File.Delete(GetPath(fileName));

            Trace(nameof(Cleanup), fileName, info, NtStatus.Success);
        }

        public void CloseFile(string fileName, DokanFileInfo info)
        {
            if (info.Context != null && _doLogging)
                Console.WriteLine(DokanFormat($"{nameof(CloseFile)}('{fileName}', {info} - entering"));

            (info.Context as FileStream)?.Dispose();
            info.Context = null;
            Trace(nameof(CloseFile), fileName, info, NtStatus.Success);

            Trace(nameof(CloseFile), fileName, info, NtStatus.Success);
        }

        public NtStatus CreateFile(string fileName, FileAccess access, FileShare share, FileMode mode,
            FileOptions options, FileAttributes attributes, DokanFileInfo info)
        {
            var result = NtStatus.Success;

            var filePath = GetPath(fileName);

            if (info.IsDirectory)
            {
                try
                {
                    switch (mode)
                    {
                        case FileMode.Open:
                            if (!Directory.Exists(filePath))
                            {
                                try
                                {
                                    if (!File.GetAttributes(filePath).HasFlag(FileAttributes.Directory))
                                        return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                                            attributes, NtStatus.NotADirectory);
                                }
                                catch (Exception)
                                {
                                    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                                        attributes, DokanResult.FileNotFound);
                                }
                                return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                                    attributes, DokanResult.PathNotFound);
                            }

                            new DirectoryInfo(filePath).EnumerateFileSystemInfos().Any();
                            // you can't list the directory
                            break;

                        case FileMode.CreateNew:
                            if (Directory.Exists(filePath))
                                return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                                    attributes, DokanResult.FileExists);

                            try
                            {
                                if (File.GetAttributes(filePath).HasFlag(FileAttributes.Directory))
                                    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                                        attributes, DokanResult.AlreadyExists);
                            }
                            catch (IOException)
                            {
                            }

                            Directory.CreateDirectory(GetPath(fileName));
                            break;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                        DokanResult.AccessDenied);
                }
            }
            else
            {
                var pathExists = true;
                var pathIsDirectory = false;
                var fileisAnnexed = false;

                var readWriteAttributes = (access & DataAccess) == 0;
                var readAccess = (access & DataWriteAccess) == 0;

                try
                {
                    pathIsDirectory = Directory.Exists(filePath);
                    pathExists = pathIsDirectory || File.Exists(filePath);
                    var fileStatus = _repository.GetFileStatus(filePath);
                    fileisAnnexed = fileStatus == GinRepository.FileStatus.InAnnex ||
                                    fileStatus == GinRepository.FileStatus.InAnnexModified;
                }
                catch (IOException)
                {
                }

                switch (mode)
                {
                    case FileMode.Open:

                        if (pathExists)
                        {
                            // check if driver only wants to read attributes, security info, or open directory
                            if (readWriteAttributes || pathIsDirectory)
                            {
                                if (pathIsDirectory && (access & FileAccess.Delete) == FileAccess.Delete
                                    && (access & FileAccess.Synchronize) != FileAccess.Synchronize)
                                    //It is a DeleteFile request on a directory
                                    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                                        attributes, DokanResult.AccessDenied);

                                info.IsDirectory = pathIsDirectory;
                                info.Context = new object();
                                // must set it to someting if you return DokanError.Success

                                return Trace(nameof(CreateFile), fileName, info, access, share, mode, options,
                                    attributes, DokanResult.Success);
                            }

                            //If the file is annexed and the driver definitely wants to open it, retrieve it.
                            if (fileisAnnexed)
                            {
                                var a = info.TryResetTimeout(30000); //Annex operations take time; 
                                var success = _repository.RetrieveFile(filePath);
                            }
                        }
                        else
                        {
                            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                                DokanResult.FileNotFound);
                        }
                        break;

                    case FileMode.CreateNew:
                        if (pathExists)
                            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                                DokanResult.FileExists);
                        break;

                    case FileMode.Truncate:
                        if (!pathExists)
                            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                                DokanResult.FileNotFound);
                        break;
                }

                try
                {
                    info.Context = new FileStream(filePath, mode,
                        readAccess ? System.IO.FileAccess.Read : System.IO.FileAccess.ReadWrite, share, 4096, options);

                    if (pathExists && (mode == FileMode.OpenOrCreate
                                       || mode == FileMode.Create))
                        result = DokanResult.AlreadyExists;

                    if (mode == FileMode.CreateNew || mode == FileMode.Create) //Files are always created as Archive
                        attributes |= FileAttributes.Archive;
                    File.SetAttributes(filePath, attributes);
                }
                catch (UnauthorizedAccessException) // don't have access rights
                {
                    if (info.Context is FileStream fileStream)
                    {
                        // returning AccessDenied cleanup and close won't be called
                        fileStream.Dispose();
                        info.Context = null;
                    }

                    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                        DokanResult.AccessDenied);
                }
                catch (DirectoryNotFoundException)
                {
                    return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                        DokanResult.PathNotFound);
                }
                catch (Exception ex)
                {
                    var hr = (uint) Marshal.GetHRForException(ex);
                    switch (hr)
                    {
                        case 0x80070020: //Sharing violation
                            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes,
                                DokanResult.SharingViolation);
                        default:
                            throw;
                    }
                }
            }

            return Trace(nameof(CreateFile), fileName, info, access, share, mode, options, attributes, result);
        }

        public NtStatus DeleteDirectory(string fileName, DokanFileInfo info)
        {
            return Trace(nameof(DeleteDirectory), fileName, info,
                Directory.EnumerateFileSystemEntries(GetPath(fileName)).Any()
                    ? DokanResult.DirectoryNotEmpty
                    : DokanResult.Success);
            // if dir is not empty it can't be deleted
        }

        public NtStatus DeleteFile(string fileName, DokanFileInfo info)
        {
            var filePath = GetPath(fileName);

            if (Directory.Exists(filePath))
                return Trace(nameof(DeleteFile), fileName, info, DokanResult.AccessDenied);

            if (!File.Exists(filePath))
                return Trace(nameof(DeleteFile), fileName, info, DokanResult.FileNotFound);

            if (File.GetAttributes(filePath).HasFlag(FileAttributes.Directory))
                return Trace(nameof(DeleteFile), fileName, info, DokanResult.AccessDenied);

            return Trace(nameof(DeleteFile), fileName, info, DokanResult.Success);
            // we just check here if we could delete the file - the true deletion is in Cleanup
        }

        public NtStatus FindFiles(string fileName, out IList<FileInformation> files, DokanFileInfo info)
        {
            // This function is not called because FindFilesWithPattern is implemented
            // Return DokanResult.NotImplemented in FindFilesWithPattern to make FindFiles called
            files = FindFilesHelper(fileName, "*");

            return Trace(nameof(FindFiles), fileName, info, DokanResult.Success);
        }

        public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files,
            DokanFileInfo info)
        {
            files = FindFilesHelper(fileName, searchPattern);

            return Trace(nameof(FindFilesWithPattern), fileName, info, DokanResult.Success);
        }

        public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, DokanFileInfo info)
        {
            streams = new FileInformation[0];
            return Trace(nameof(FindStreams), fileName, info, DokanResult.NotImplemented);
        }

        public NtStatus FlushFileBuffers(string fileName, DokanFileInfo info)
        {
            try
            {
                ((FileStream) info.Context).Flush();
                return Trace(nameof(FlushFileBuffers), fileName, info, DokanResult.Success);
            }
            catch (IOException)
            {
                return Trace(nameof(FlushFileBuffers), fileName, info, DokanResult.DiskFull);
            }
        }

        public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes,
            out long totalNumberOfFreeBytes, DokanFileInfo info)
        {
            var dinfo = DriveInfo.GetDrives().Single(di => string.Equals(di.RootDirectory.Name,
                Path.GetPathRoot(_repository.PhysicalDirectory + "\\"), StringComparison.OrdinalIgnoreCase));

            freeBytesAvailable = dinfo.TotalFreeSpace;
            totalNumberOfBytes = dinfo.TotalSize;
            totalNumberOfFreeBytes = dinfo.AvailableFreeSpace;
            return Trace(nameof(GetDiskFreeSpace), null, info, DokanResult.Success, "out " + freeBytesAvailable,
                "out " + totalNumberOfBytes, "out " + totalNumberOfFreeBytes);
        }

        public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, DokanFileInfo info)
        {
            // may be called with info.Context == null, but usually it isn't
            var filePath = GetPath(fileName);
            FileSystemInfo finfo = new FileInfo(filePath);
            if (!finfo.Exists)
                finfo = new DirectoryInfo(filePath);

            fileInfo = new FileInformation
            {
                FileName = fileName,
                Attributes = finfo.Attributes,
                CreationTime = finfo.CreationTime,
                LastAccessTime = finfo.LastAccessTime,
                LastWriteTime = finfo.LastWriteTime,
                Length = (finfo as FileInfo)?.Length ?? 0
            };

            return Trace(nameof(GetFileInformation), fileName, info, DokanResult.Success);
        }

        public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security,
            AccessControlSections sections, DokanFileInfo info)
        {
            try
            {
                security = info.IsDirectory
                    ? (FileSystemSecurity) Directory.GetAccessControl(GetPath(fileName))
                    : File.GetAccessControl(GetPath(fileName));
                return Trace(nameof(GetFileSecurity), fileName, info, DokanResult.Success, sections.ToString());
            }
            catch (UnauthorizedAccessException)
            {
                security = null;
                return Trace(nameof(GetFileSecurity), fileName, info, DokanResult.AccessDenied, sections.ToString());
            }
        }

        public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features,
            out string fileSystemName, DokanFileInfo info)
        {
            volumeLabel = _repository.Mountpoint.FullName + Path.DirectorySeparatorChar + _repository.Name;
            fileSystemName = "NTFS";
            features = FileSystemFeatures.CasePreservedNames | FileSystemFeatures.CaseSensitiveSearch |
                       FileSystemFeatures.PersistentAcls | FileSystemFeatures.SupportsRemoteStorage |
                       FileSystemFeatures.UnicodeOnDisk;

            return Trace(nameof(GetVolumeInformation), null, info, DokanResult.Success, "out " + volumeLabel,
                "out " + features, "out " + fileSystemName);
        }

        public NtStatus LockFile(string fileName, long offset, long length, DokanFileInfo info)
        {
            try
            {
                ((FileStream) info.Context).Lock(offset, length);
                return Trace(nameof(LockFile), fileName, info, DokanResult.Success,
                    offset.ToString(CultureInfo.InvariantCulture), length.ToString(CultureInfo.InvariantCulture));
            }
            catch (IOException)
            {
                return Trace(nameof(LockFile), fileName, info, DokanResult.AccessDenied,
                    offset.ToString(CultureInfo.InvariantCulture), length.ToString(CultureInfo.InvariantCulture));
            }
        }

        public NtStatus Mounted(DokanFileInfo info)
        {
            var result = NtStatus.Success;

            return Trace(nameof(Mounted), null, info, result);
        }

        public NtStatus MoveFile(string oldName, string newName, bool replace, DokanFileInfo info)
        {
            var oldpath = GetPath(oldName);
            var newpath = GetPath(newName);

            (info.Context as FileStream)?.Dispose();
            info.Context = null;

            var exist = info.IsDirectory ? Directory.Exists(newpath) : File.Exists(newpath);

            try
            {
                if (!exist)
                {
                    info.Context = null;
                    if (info.IsDirectory)
                        Directory.Move(oldpath, newpath);
                    else
                        File.Move(oldpath, newpath);
                    return Trace(nameof(MoveFile), oldName, info, DokanResult.Success, newName,
                        replace.ToString(CultureInfo.InvariantCulture));
                }
                if (replace)
                {
                    info.Context = null;

                    if (info.IsDirectory) //Cannot replace directory destination - See MOVEFILE_REPLACE_EXISTING
                        return Trace(nameof(MoveFile), oldName, info, DokanResult.AccessDenied, newName,
                            replace.ToString(CultureInfo.InvariantCulture));

                    File.Delete(newpath);
                    File.Move(oldpath, newpath);
                    return Trace(nameof(MoveFile), oldName, info, DokanResult.Success, newName,
                        replace.ToString(CultureInfo.InvariantCulture));
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Trace(nameof(MoveFile), oldName, info, DokanResult.AccessDenied, newName,
                    replace.ToString(CultureInfo.InvariantCulture));
            }
            return Trace(nameof(MoveFile), oldName, info, DokanResult.FileExists, newName,
                replace.ToString(CultureInfo.InvariantCulture));
        }

        public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, DokanFileInfo info)
        {
            if (info.Context == null) // memory mapped read
            {
                using (var stream = new FileStream(GetPath(fileName), FileMode.Open, System.IO.FileAccess.Read))
                {
                    stream.Position = offset;
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
            }
            else // normal read
            {
                var stream = info.Context as FileStream;
                lock (stream) //Protect from overlapped read
                {
                    stream.Position = offset;
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
            }
            return Trace(nameof(ReadFile), fileName, info, NtStatus.Success, "out " + bytesRead,
                offset.ToString(CultureInfo.InvariantCulture));
        }

        public NtStatus SetAllocationSize(string fileName, long length, DokanFileInfo info)
        {
            try
            {
                ((FileStream) info.Context).SetLength(length);
                return Trace(nameof(SetAllocationSize), fileName, info, DokanResult.Success,
                    length.ToString(CultureInfo.InvariantCulture));
            }
            catch (IOException)
            {
                return Trace(nameof(SetAllocationSize), fileName, info, DokanResult.DiskFull,
                    length.ToString(CultureInfo.InvariantCulture));
            }
        }

        public NtStatus SetEndOfFile(string fileName, long length, DokanFileInfo info)
        {
            try
            {
                ((FileStream) info.Context).SetLength(length);
                return Trace(nameof(SetEndOfFile), fileName, info, DokanResult.Success,
                    length.ToString(CultureInfo.InvariantCulture));
            }
            catch (IOException)
            {
                return Trace(nameof(SetEndOfFile), fileName, info, DokanResult.DiskFull,
                    length.ToString(CultureInfo.InvariantCulture));
            }
        }

        public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, DokanFileInfo info)
        {
            try
            {
                File.SetAttributes(GetPath(fileName), attributes);
                return Trace(nameof(SetFileAttributes), fileName, info, DokanResult.Success, attributes.ToString());
            }
            catch (UnauthorizedAccessException)
            {
                return Trace(nameof(SetFileAttributes), fileName, info, DokanResult.AccessDenied,
                    attributes.ToString());
            }
            catch (FileNotFoundException)
            {
                return Trace(nameof(SetFileAttributes), fileName, info, DokanResult.FileNotFound,
                    attributes.ToString());
            }
            catch (DirectoryNotFoundException)
            {
                return Trace(nameof(SetFileAttributes), fileName, info, DokanResult.PathNotFound,
                    attributes.ToString());
            }
        }

        public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections,
            DokanFileInfo info)
        {
            try
            {
                if (info.IsDirectory)
                    Directory.SetAccessControl(GetPath(fileName), (DirectorySecurity) security);
                else
                    File.SetAccessControl(GetPath(fileName), (FileSecurity) security);
                return Trace(nameof(SetFileSecurity), fileName, info, DokanResult.Success, sections.ToString());
            }
            catch (UnauthorizedAccessException)
            {
                return Trace(nameof(SetFileSecurity), fileName, info, DokanResult.AccessDenied, sections.ToString());
            }
        }

        public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime,
            DateTime? lastWriteTime, DokanFileInfo info)
        {
            try
            {
                if (info.Context is FileStream stream)
                {
                    var ct = creationTime?.ToFileTime() ?? 0;
                    var lat = lastAccessTime?.ToFileTime() ?? 0;
                    var lwt = lastWriteTime?.ToFileTime() ?? 0;
                    if (NativeMethods.SetFileTime(stream.SafeFileHandle, ref ct, ref lat, ref lwt))
                        return NtStatus.Success;
                    throw Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());
                }

                var filePath = GetPath(fileName);

                if (creationTime.HasValue)
                    File.SetCreationTime(filePath, creationTime.Value);

                if (lastAccessTime.HasValue)
                    File.SetLastAccessTime(filePath, lastAccessTime.Value);

                if (lastWriteTime.HasValue)
                    File.SetLastWriteTime(filePath, lastWriteTime.Value);

                return Trace(nameof(SetFileTime), fileName, info, DokanResult.Success, creationTime, lastAccessTime,
                    lastWriteTime);
            }
            catch (UnauthorizedAccessException)
            {
                return Trace(nameof(SetFileTime), fileName, info, DokanResult.AccessDenied, creationTime,
                    lastAccessTime,
                    lastWriteTime);
            }
            catch (FileNotFoundException)
            {
                return Trace(nameof(SetFileTime), fileName, info, DokanResult.FileNotFound, creationTime,
                    lastAccessTime,
                    lastWriteTime);
            }
        }

        public NtStatus UnlockFile(string fileName, long offset, long length, DokanFileInfo info)
        {
            try
            {
                ((FileStream) info.Context).Unlock(offset, length);
                return Trace(nameof(UnlockFile), fileName, info, DokanResult.Success,
                    offset.ToString(CultureInfo.InvariantCulture), length.ToString(CultureInfo.InvariantCulture));
            }
            catch (IOException)
            {
                return Trace(nameof(UnlockFile), fileName, info, DokanResult.AccessDenied,
                    offset.ToString(CultureInfo.InvariantCulture), length.ToString(CultureInfo.InvariantCulture));
            }
        }

        public NtStatus Unmounted(DokanFileInfo info)
        {
            var result = NtStatus.Success;

            return Trace(nameof(Unmounted), null, info, result);
        }

        public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, DokanFileInfo info)
        {
            if (info.Context == null)
            {
                using (var stream = new FileStream(GetPath(fileName), FileMode.Open, System.IO.FileAccess.Write))
                {
                    stream.Position = offset;
                    stream.Write(buffer, 0, buffer.Length);
                    bytesWritten = buffer.Length;
                }
            }
            else
            {
                var stream = info.Context as FileStream;
                lock (stream) //Protect from overlapped write
                {
                    stream.Position = offset;
                    stream.Write(buffer, 0, buffer.Length);
                }
                bytesWritten = buffer.Length;
            }
            return Trace(nameof(WriteFile), fileName, info, DokanResult.Success, "out " + bytesWritten,
                offset.ToString(CultureInfo.InvariantCulture));
        }

        #endregion
    }
}