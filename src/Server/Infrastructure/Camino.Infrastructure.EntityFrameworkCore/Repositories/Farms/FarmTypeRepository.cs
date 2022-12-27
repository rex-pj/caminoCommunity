using System;
using System.Threading.Tasks;
using Camino.Core.Domains.Farms.Repositories;
using Camino.Core.Domains.Farms;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Farms
{
    public class FarmTypeRepository : IFarmTypeRepository, IScopedDependency
    {
        private readonly IEntityRepository<FarmType> _farmTypeRepository;
        private readonly IDbContext _dbContext;

        public FarmTypeRepository(IEntityRepository<FarmType> farmTypeRepository, IDbContext dbContext)
        {
            _farmTypeRepository = farmTypeRepository;
            _dbContext = dbContext;
        }

        public async Task<FarmType> FindAsync(long id)
        {
            var exist = await _farmTypeRepository.FindAsync(x => x.Id == id);
            return exist;
        }

        public async Task<FarmType> FindByNameAsync(string name)
        {
            var farmType = await _farmTypeRepository.FindAsync(x => x.Name == name);
            return farmType;
        }

        public async Task<long> CreateAsync(FarmType farmType)
        {
            var modifiedDate = DateTime.UtcNow;
            farmType.CreatedDate = modifiedDate;
            farmType.UpdatedDate = modifiedDate;
            await _farmTypeRepository.InsertAsync(farmType);
            await _dbContext.SaveChangesAsync();
            return farmType.Id;
        }

        public async Task<bool> UpdateAsync(FarmType farmType)
        {
            farmType.UpdatedDate = DateTime.UtcNow;
            await _farmTypeRepository.UpdateAsync(farmType);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _farmTypeRepository.DeleteAsync(x => x.Id == id);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}
