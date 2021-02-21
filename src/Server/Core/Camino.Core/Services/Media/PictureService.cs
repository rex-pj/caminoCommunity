using Camino.Core.Contracts.Services.Media;
using Camino.Shared.Results.Media;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Media;

namespace Camino.Services.Media
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;

        public PictureService(IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public async Task<PictureResult> FindPictureAsync(long id)
        {
            var picture = await _pictureRepository.FindPictureAsync(id);
            return picture;
        }
    }
}
