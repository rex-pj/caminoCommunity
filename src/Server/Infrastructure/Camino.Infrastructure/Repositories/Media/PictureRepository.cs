using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Media;
using Camino.Shared.Results.Media;
using LinqToDB;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Domain.Media;
using Camino.Shared.Requests.Media;

namespace Camino.Service.Repository.Media
{
    public class PictureRepository : IPictureRepository
    {
        private readonly IRepository<Picture> _pictureRepository;

        public PictureRepository(IRepository<Picture> pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public async Task<PictureResult> FindPictureAsync(long id)
        {
            var picture = await _pictureRepository.Get(x => x.Id == id).Select(pic => new PictureResult
            {
                Id = pic.Id,
                FileName = pic.FileName,
                BinaryData = pic.BinaryData,
                ContentType = pic.MimeType
            }).FirstOrDefaultAsync();

            if (picture == null)
            {
                return null;
            }

            return picture;
        }
    }
}
