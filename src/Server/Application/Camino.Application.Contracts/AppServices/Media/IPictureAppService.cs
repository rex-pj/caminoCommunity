using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Camino.Application.Contracts.AppServices.Media
{
    public interface IPictureAppService
    {
        Task<PictureResult> FindAsync(IdRequestFilter<long> filter);
    }
}