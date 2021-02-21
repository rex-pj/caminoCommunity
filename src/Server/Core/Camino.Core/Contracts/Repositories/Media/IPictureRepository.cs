using Camino.Shared.Requests.Media;
using Camino.Shared.Results.Media;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Media
{
    public interface IPictureRepository
    {
        Task<PictureResult> FindPictureAsync(long id);
    }
}