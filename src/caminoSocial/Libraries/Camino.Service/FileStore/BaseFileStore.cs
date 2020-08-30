using Camino.Service.Data.FileEntry;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Service.FileStore
{
    public abstract class BaseFileStore
    {
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

        public abstract Task<string> CreateFileFromStreamAsync(string path, Stream inputStream, bool overwrite = false);
        public abstract Task<FileEntryInfo> GetFileInfoAsync(string path);
        public abstract Task<string> CreateFileAsync(string path, Stream inputStream, bool overwrite = false);
        public abstract string MapPathToPublicUrl(string path);
    }
}
