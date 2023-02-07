using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

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

        public static bool ValidateName(string fileName)
        {
            return new Regex(@"^[0-9a-zA-Z\-_.()#@!-$%^&' ]+$").IsMatch(fileName);
        }

        public static bool ValidateExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".svg":
                case ".gif":
                    return true;
                default:
                    return false;
            }
        }

        public static bool ValidateContentType(string contentType)
        {
            switch (contentType.ToLower())
            {
                case "image/jpeg":
                case "image/png":
                case "image/svg+xml":
                case "image/gif":
                    return true;
                default:
                    return false;
            }
        }

        public static bool ValidateContentTypeAndExtension(string contentType, string extension)
        {
            extension = extension.ToLower();
            switch (contentType.ToLower())
            {
                case "image/jpeg":
                    return extension.Equals(".jpeg") || extension.Equals(".jpg");
                case "image/png":
                    return extension.Equals(".png");
                case "image/svg+xml":
                    return extension.Equals(".svg");
                case "image/gif":
                    return extension.Equals(".gif");
                default:
                    return false;
            }
        }
    }
}
