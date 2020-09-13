using Microsoft.Extensions.FileProviders;
using System;

namespace Camino.Service.Projections.FileEntry
{
    public class FileEntryInfo
    {
        private readonly IFileInfo _fileInfo;

        internal FileEntryInfo(string path, IFileInfo fileInfo)
        {
            _fileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public string Path { get; }
        public string Name
        {
            get
            {
                return _fileInfo.Name;
            }
        }

        public string DirectoryPath
        {
            get
            {
                return Path.Substring(0, Path.Length - Name.Length).TrimEnd('/');
            }
        }

        public DateTime LastModifiedUtc
        {
            get
            {
                return _fileInfo.LastModified.UtcDateTime;
            }
        }

        public long Length
        {
            get
            {
                return _fileInfo.Length;
            }
        }

        public bool IsDirectory
        {
            get
            {
                return _fileInfo.IsDirectory;
            }
        }
    }
}
