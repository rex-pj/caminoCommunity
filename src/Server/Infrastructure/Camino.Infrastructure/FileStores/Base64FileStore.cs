using Camino.Core.Contracts.FileStore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Camino.Infrastructure.FileStores
{
    public class Base64FileStore : IFileStore
    {
        public async Task<string> CreateFileAsync(Stream inputStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await inputStream.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }
    }
}
