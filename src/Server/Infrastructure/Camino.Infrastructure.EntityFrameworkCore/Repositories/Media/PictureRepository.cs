using Camino.Core.Contracts.Repositories.Media;
using System.Threading.Tasks;
using Camino.Core.Domains.Media;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using System;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Media
{
    public class PictureRepository : IPictureRepository, IScopedDependency
    {
        private readonly IEntityRepository<Picture> _pictureRepository;
        private readonly IAppDbContext _dbContext;

        public PictureRepository(IEntityRepository<Picture> pictureRepository, IAppDbContext dbContext)
        {
            _pictureRepository = pictureRepository;
            _dbContext = dbContext;
        }

        public async Task<Picture> FindAsync(long id)
        {
            using (_pictureRepository)
            {
                var picture = await _pictureRepository
                .FindAsync(x => x.Id == id);
                return picture;
            }
        }

        public async Task<long> CreateAsync(Picture picture)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            picture.CreatedDate = modifiedDate;
            picture.UpdatedDate = modifiedDate;
            await _pictureRepository.InsertAsync(picture);
            await _dbContext.SaveChangesAsync();

            return picture.Id;
        }
    }
}
