﻿using Camino.Shared.Results.FileEntry;
using System.IO;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.FileStore
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
