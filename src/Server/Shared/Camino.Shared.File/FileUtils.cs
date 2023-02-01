using Microsoft.AspNetCore.Http;

namespace Camino.Shared.File
{
    public static class FileUtils
    {
        public static async Task<byte[]> GetBytesAsync(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                throw new FileNotFoundException("The file is null or empty", nameof(file));
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
