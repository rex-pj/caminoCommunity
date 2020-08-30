using Camino.Core.Constants;
using Camino.Core.Exceptions;
using Camino.Service.Data.FileEntry;
using Microsoft.Extensions.FileProviders.Physical;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Camino.Service.FileStore
{
    public class LocalMediaFileStore : BaseFileStore
    {
        private readonly string _mediaFullPath;
        private readonly string _mediaPath;

        public LocalMediaFileStore(string appDataPath)
        {
            var appDataFullPath = Path.GetFullPath(appDataPath);
            _mediaPath = AppDataSettings.MediaPath;
            _mediaFullPath = Path.Combine(appDataFullPath, AppDataSettings.MediaPath);
        }

        public override async Task<string> CreateFileAsync(string path, Stream inputStream, bool overwrite = false)
        {
            return await CreateFileFromStreamAsync(path, inputStream, overwrite);
        }

        public override async Task<string> CreateFileFromStreamAsync(string path, Stream inputStream, bool overwrite = false)
        {
            var physicalPath = GetPhysicalPath(path);

            if (!overwrite && File.Exists(physicalPath))
            {
                throw new FileStoreException($"Cannot create file '{path}' because it already exists.");
            }

            if (Directory.Exists(physicalPath))
            {
                throw new FileStoreException($"Cannot create file '{path}' because it already exists as a directory.");
            }

            var physicalDirectoryPath = Path.GetDirectoryName(physicalPath);
            Directory.CreateDirectory(physicalDirectoryPath);

            var fileInfo = new FileInfo(physicalPath);
            using (var outputStream = fileInfo.Create())
            {
                await inputStream.CopyToAsync(outputStream);
            }

            return path;
        }

        private string GetPhysicalPath(string path)
        {
            var physicalPath = string.IsNullOrEmpty(path) ? _mediaFullPath : Path.Combine(_mediaFullPath, path);

            var pathIsAllowed = Path.GetFullPath(physicalPath).StartsWith(_mediaFullPath, StringComparison.OrdinalIgnoreCase);
            if (!pathIsAllowed)
            {
                throw new FileStoreException($"The path '{path}' resolves to a physical path outside the file system store root.");
            }

            return physicalPath;
        }

        public override async Task<FileEntryInfo> GetFileInfoAsync(string path)
        {
            var physicalPath = GetPhysicalPath(path);
            var fileInfo = new PhysicalFileInfo(new FileInfo(physicalPath));

            if (fileInfo.Exists)
            {
                return await Task.FromResult(new FileEntryInfo(path, fileInfo));
            }

            return null;
        }

        public override string MapPathToPublicUrl(string path)
        {
            return _mediaPath + "/" + NormalizePath(path);
        }
    }
}
