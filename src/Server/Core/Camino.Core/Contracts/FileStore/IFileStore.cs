using System.IO;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.FileStore
{
    public interface IFileStore
    {
        Task<string> CreateFileAsync(Stream inputStream);
    }
}
