using System.Text;

namespace Camino.Infrastructure.Files.Contracts
{
    public interface IFileProvider
    {
        string ReadText(string path, Encoding encoding);
        bool FileExists(string filePath);
        void CreateFile(string path, string text);
        void CreateDirectory(string path);
        bool DirectoryExists(string path);
        void DeleteFile(string filePath);
    }
}
