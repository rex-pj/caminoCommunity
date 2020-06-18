using System.Text;

namespace Coco.Framework.Services.Contracts
{
    public interface IFileAccessor
    {
        string ReadAllText(string path, Encoding encoding);
        void WriteAllText(string path, string contents, Encoding encoding);
        bool FileExists(string filePath);
        void CreateFile(string path);
        void CreateDirectory(string path);
        bool DirectoryExists(string path);
    }
}
