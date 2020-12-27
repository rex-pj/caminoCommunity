using System.IO;
using System.Threading.Tasks;

namespace Camino.Service.FileStore.Contracts
{
    public interface IFileStore
    {
        Task<string> CreateFileAsync(Stream inputStream);
    }
}
