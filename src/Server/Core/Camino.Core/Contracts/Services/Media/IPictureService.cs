using Camino.Shared.Results.Media;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Services.Media
{
    public interface IPictureService
    {
        Task<PictureResult> FindPictureAsync(long id);
    }
}