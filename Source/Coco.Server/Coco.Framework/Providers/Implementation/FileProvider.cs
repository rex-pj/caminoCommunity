using Coco.Framework.Providers.Contracts;
using System.IO;
using System.Text;

namespace Coco.Framework.Providers.Implementation
{
    public class FileProvider : IFileProvider
    {
        public void WriteText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents, encoding);
        }

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

        public void CreateFile(string path)
        {
            if (FileExists(path))
            {
                return;
            }

            var fileInfo = new FileInfo(path);
            CreateDirectory(fileInfo.DirectoryName);

            File.Create(path).Close();
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
