using Camino.Core.Contracts.Services.Media;
using Camino.Shared.Results.Media;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Media;
using Camino.Shared.Requests.Filters;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Services.Media
{
    public class PictureService : IPictureService, IScopedDependency
    {
        private readonly IPictureRepository _pictureRepository;

        public PictureService(IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public async Task<PictureResult> FindAsync(IdRequestFilter<long> filter)
        {
            var picture = await _pictureRepository.FindAsync(filter);
            return picture;
        }
    }
}
