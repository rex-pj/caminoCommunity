using Camino.Infrastructure.Files.Contracts;

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
