using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.Media;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Media
{
    public interface IPictureService
    {
        Task<PictureResult> FindAsync(IdRequestFilter<long> filter);
    }
}