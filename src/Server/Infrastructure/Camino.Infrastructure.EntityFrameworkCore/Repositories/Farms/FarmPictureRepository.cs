using Camino.Core.Domains.Farms.Repositories;
using Camino.Core.Domains.Farms;
using System.Threading.Tasks;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;
using Camino.Shared.Enums;
using Camino.Shared.Utils;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Farms
{
    public class FarmPictureRepository : IFarmPictureRepository, IScopedDependency
    {
        private readonly IEntityRepository<FarmPicture> _farmPictureRepository;
        private readonly IAppDbContext _dbContext;

        public FarmPictureRepository(IEntityRepository<FarmPicture> farmPictureRepository, IAppDbContext dbContext)
        {
            _farmPictureRepository = farmPictureRepository;
            _dbContext = dbContext;
        }

        public async Task<FarmPicture> GetByTypeAsync(long farmId, FarmPictureTypes pictureType)
        {
            return await _farmPictureRepository
                .FindAsync(x => x.FarmId == farmId && x.PictureTypeId == pictureType.GetCode());
        }

        public async Task<long> CreateAsync(FarmPicture productPicture, bool needSaveChanges = false)
        {
            await _farmPictureRepository.InsertAsync(productPicture);
            if (needSaveChanges)
            {
                await _dbContext.SaveChangesAsync();
                return productPicture.Id;
            }
            return -1;
        }

        public async Task UpdateAsync(FarmPicture productPicture, bool needSaveChanges = false)
        {
            await _farmPictureRepository.UpdateAsync(productPicture);
            if (needSaveChanges)
            {
                return;
            }
        }
    }
}
