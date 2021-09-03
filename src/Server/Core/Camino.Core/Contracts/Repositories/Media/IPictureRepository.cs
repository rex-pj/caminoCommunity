using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Media;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Media
{
    public interface IPictureRepository
    {
        Task<PictureResult> FindAsync(IdRequestFilter<long> filter);
    }
}