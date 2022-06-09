using Camino.Core.Domains.Media;

namespace Camino.Core.Contracts.Repositories.Media
{
    public interface IPictureRepository
    {
        Task<Picture> FindAsync(long id);
        Task<long> CreateAsync(Picture picture);
    }
}