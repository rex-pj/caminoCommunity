using Camino.Core.Constants;
using Camino.Core.Exceptions;
using Camino.Service.Data.FileEntry;
using Camino.Service.FileStore.Contracts;
using Microsoft.Extensions.FileProviders.Physical;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Service.FileStore
{
    public class LocalMediaFileStore : IMediaFileStore
    {
        private readonly string _mediaFullPath;
        private readonly string _mediaPath;

        public LocalMediaFileStore(string appDataPath)
        {
            var appDataFullPath = Path.GetFullPath(appDataPath);
            _mediaPath = AppDataSettings.MediaPath;
            _mediaFullPath = Path.Combine(appDataFullPath, AppDataSettings.MediaPath);
        }

        public async Task<string> CreateFileAsync(string path, Stream inputStream, bool overwrite = false)
        {
            return await CreateFileFromStreamAsync(path, inputStream, overwrite);
        }

        public async Task<string> CreateFileFromStreamAsync(string path, Stream inputStream, bool overwrite = false)
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

        public async Task<FileEntryInfo> GetFileInfoAsync(string path)
        {
            var physicalPath = GetPhysicalPath(path);
            var fileInfo = new PhysicalFileInfo(new FileInfo(physicalPath));

            if (fileInfo.Exists)
            {
                return await Task.FromResult(new FileEntryInfo(path, fileInfo));
            }

            return null;
        }

        public string MapPathToPublicUrl(string path)
        {
            return _mediaPath + "/" + NormalizePath(path);
        }

        public virtual string Combine(params string[] paths)
        {
            if (!paths.Any())
            {
                return null;
            }

            var normalizedPaths = paths.Select(x => NormalizePath(x))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();

            var combinedPaths = string.Join("/", normalizedPaths);

            if (paths[0]?.StartsWith('/') is true)
            {
                combinedPaths = "/" + combinedPaths;
            }

            return combinedPaths;
        }

        public string NormalizePath(string path)
        {
            if (path == null)
            {
                return null;
            }

            return path.Replace('\\', '/').Trim('/');
        }
    }
}
