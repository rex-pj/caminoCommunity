using Camino.Core.Domains.Farms.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Core.Domains.Farms;
using Camino.Core.Domains;
using Camino.Core.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Farms
{
    public class FarmRepository : IFarmRepository, IScopedDependency
    {
        private readonly IEntityRepository<Farm> _farmRepository;
        
        private readonly IAppDbContext _dbContext;

        public FarmRepository(IEntityRepository<Farm> farmRepository, IAppDbContext dbContext)
        {
            _farmRepository = farmRepository;
            _dbContext = dbContext;
        }

        public async Task<Farm> FindAsync(long id)
        {
            var exist = await _farmRepository.FindAsync(x => x.Id == id);
            return exist;
        }

        public async Task<IList<Farm>> GetByTypeAsync(long farmType)
        {
            return await _farmRepository.GetAsync(x => x.FarmTypeId == farmType);
        }

        public async Task<Farm> FindByNameAsync(string name)
        {
            var exist = await _farmRepository.FindAsync(x => x.Name == name);
            return exist;
        }

        public async Task<long> CreateAsync(Farm farm)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            farm.UpdatedDate = modifiedDate;
            farm.CreatedDate = modifiedDate;
            await _farmRepository.InsertAsync(farm);
            await _dbContext.SaveChangesAsync();
            return farm.Id;
        }

        public async Task<bool> UpdateAsync(Farm farm)
        {
            var updatedDate = DateTimeOffset.UtcNow;
            farm.UpdatedDate = updatedDate;
            await _farmRepository.UpdateAsync(farm);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // Delete farm
            await _farmRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
