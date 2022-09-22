using Microsoft.AspNetCore.Http;

namespace Camino.Shared.File
{
    public static class FileUtils
    {
        public static async Task<byte[]> GetBytesAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }

            throw new FileNotFoundException("The file is null or empty", nameof(file));
        }

        public static async Task<string> GetBase64Async(IFormFile file)
        {
            var bytes = await GetBytesAsync(file);
            return Convert.ToBase64String(bytes);
        }
    }
}
