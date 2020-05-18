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
    }
}
