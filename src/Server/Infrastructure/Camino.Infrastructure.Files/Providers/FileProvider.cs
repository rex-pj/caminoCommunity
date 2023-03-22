using Camino.Infrastructure.Files.Contracts;
using System.Text;

namespace Camino.Infrastructure.Providers
{
    public class FileProvider : IFileProvider
    {
        public string ReadText(string path, Encoding encoding)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fileStream, encoding))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public void CreateFile(string path, string text)
        {
            using (var writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.Write(text);
            }
        }

        public void CreateDirectory(string path)
        {
            if (!DirectoryExists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}
