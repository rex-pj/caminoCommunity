using Camino.Core.Contracts.Repositories.Media;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Core.Domains.Media;
using Camino.Application.Contracts;
using Camino.Shared.Enums;
using Camino.Application.Contracts.AppServices.Media;
using Camino.Core.DependencyInjection;
using System.Threading.Tasks;

namespace Camino.Application.AppServices.Media
{
    public class PictureAppService : IPictureAppService, IScopedDependency
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly int _pictureDeletedStatus;
        private readonly int _pictureInactivedStatus;

        public PictureAppService(IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
            _pictureDeletedStatus = (int)PictureStatuses.Deleted;
            _pictureInactivedStatus = (int)PictureStatuses.Inactived;
        }

        public async Task<PictureResult> FindAsync(IdRequestFilter<long> filter)
        {
            var x = await _pictureRepository.FindAsync(filter.Id);
            if (x.StatusId == _pictureDeletedStatus && filter.CanGetDeleted)
            {
                return MapEntityToDto(x);
            }
            if (x.StatusId == _pictureInactivedStatus && filter.CanGetInactived)
            {
                return MapEntityToDto(x);
            }
            if (x.StatusId != _pictureDeletedStatus && x.StatusId != _pictureInactivedStatus)
            {
                return MapEntityToDto(x);
            }
            return null;
        }

        private PictureResult MapEntityToDto(Picture pic)
        {
            return new PictureResult
            {
                Id = pic.Id,
                FileName = pic.FileName,
                BinaryData = pic.BinaryData,
                ContentType = pic.ContentType,
                CreatedById = pic.CreatedById,
                UpdatedById = pic.UpdatedById
            };
        }
    }
}
