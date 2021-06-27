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

namespace Camino.Infrastructure.Repositories.Media
{
    public class PictureRepository : IPictureRepository
    {
        private readonly IRepository<Picture> _pictureRepository;

        public PictureRepository(IRepository<Picture> pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public async Task<PictureResult> FindAsync(IdRequestFilter<long> filter)
        {
            var deletedStatus = PictureStatus.Deleted.GetCode();
            var inactivedStatus = PictureStatus.Inactived.GetCode();
            var picture = await _pictureRepository
                .Get(x => x.Id == filter.Id)
                .Where(x => (x.StatusId == deletedStatus && filter.CanGetDeleted)
                            || (x.StatusId == inactivedStatus && filter.CanGetInactived)
                            || (x.StatusId != deletedStatus && x.StatusId != inactivedStatus))
                .Select(pic => new PictureResult
                {
                    Id = pic.Id,
                    FileName = pic.FileName,
                    BinaryData = pic.BinaryData,
                    ContentType = pic.MimeType,
                    CreatedById = pic.CreatedById,
                    UpdatedById = pic.UpdatedById
                }).FirstOrDefaultAsync();

            if (picture == null)
            {
                return null;
            }

            return picture;
        }
    }
}
