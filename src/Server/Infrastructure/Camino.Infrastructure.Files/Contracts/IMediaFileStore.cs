using Camino.Infrastructure.Files.Contracts.Dtos;

namespace Camino.Infrastructure.Files.Contracts
{
    public interface IMediaFileStore
    {
        string Combine(params string[] paths);
        string NormalizePath(string path);
        string MapPathToPublicUrl(string path);
        Task<string> CreateFileFromStreamAsync(string path, Stream inputStream, bool overwrite = false);
        Task<FileEntryInfo> GetFileInfoAsync(string path);
        Task<string> CreateFileAsync(string path, Stream inputStream, bool overwrite = false);
    }
}
