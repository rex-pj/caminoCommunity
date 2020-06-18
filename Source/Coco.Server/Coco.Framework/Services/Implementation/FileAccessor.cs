using Coco.Framework.Services.Contracts;
using System.IO;
using System.Text;

namespace Coco.Framework.Services.Implementation
{
    public class FileAccessor : IFileAccessor
    {
        public string ReadAllText(string path, Encoding encoding)
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

        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents, encoding);
        }
    }
}
