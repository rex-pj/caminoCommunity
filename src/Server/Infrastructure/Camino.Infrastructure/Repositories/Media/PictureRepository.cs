using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Media;
using Camino.Shared.Results.Media;
using LinqToDB;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Domain.Media;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Infrastructure.Repositories.Media
{
    public class PictureRepository : IPictureRepository, IScopedDependency
    {
        private readonly IRepository<Picture> _pictureRepository;
        private readonly int _pictureDeletedStatus;
        private readonly int _pictureInactivedStatus;

        public PictureRepository(IRepository<Picture> pictureRepository)
        {
            _pictureRepository = pictureRepository;
            _pictureDeletedStatus = (int)PictureStatus.Deleted;
            _pictureInactivedStatus = (int)PictureStatus.Inactived;
        }

        public async Task<PictureResult> FindAsync(IdRequestFilter<long> filter)
        {
            using (_pictureRepository)
            {
                var picture = await _pictureRepository
                .Get(x => x.Id == filter.Id)
                .Where(x => (x.StatusId == _pictureDeletedStatus && filter.CanGetDeleted)
                    || (x.StatusId == _pictureInactivedStatus && filter.CanGetInactived)
                    || (x.StatusId != _pictureDeletedStatus && x.StatusId != _pictureInactivedStatus))
                .Select(pic => new PictureResult
                {
                    Id = pic.Id,
                    FileName = pic.FileName,
                    BinaryData = pic.BinaryData,
                    ContentType = pic.MimeType,
                    CreatedById = pic.CreatedById,
                    UpdatedById = pic.UpdatedById
                }).FirstOrDefaultAsync();

                return picture;
            }
        }
    }
}
