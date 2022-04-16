using Camino.Core.Contracts.Data;
using Camino.Shared.Requests.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Farms;
using Camino.Core.Contracts.Repositories.Farms;
using Camino.Core.Domain.Farms;
using Camino.Shared.Requests.Farms;
using Camino.Shared.Enums;
using Camino.Core.Utils;
using Camino.Core.Contracts.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Camino.Infrastructure.EntityFrameworkCore.Extensions;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Farms
{
    public class FarmTypeRepository : IFarmTypeRepository, IScopedDependency
    {
        private readonly IEntityRepository<FarmType> _farmTypeRepository;
        private readonly IAppDbContext _dbContext;

        public FarmTypeRepository(IEntityRepository<FarmType> farmTypeRepository, IAppDbContext dbContext)
        {
            _farmTypeRepository = farmTypeRepository;
            _dbContext = dbContext;
        }

        public async Task<FarmTypeResult> FindAsync(long id)
        {
            var exist = await (from farmType in _farmTypeRepository.Table
                         where farmType.Id == id
                         select new FarmTypeResult
                         {
                             Description = farmType.Description,
                             CreatedDate = farmType.CreatedDate,
                             CreatedById = farmType.CreatedById,
                             Id = farmType.Id,
                             Name = farmType.Name,
                             UpdatedById = farmType.UpdatedById,
                             UpdatedDate = farmType.UpdatedDate,
                             StatusId = farmType.StatusId
                         }).FirstOrDefaultAsync();

            return exist;
        }

        public FarmTypeResult FindByName(string name)
        {
            var farmType = _farmTypeRepository.Get(x => x.Name == name)
                .Select(x => new FarmTypeResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    StatusId = x.StatusId
                })
                .FirstOrDefault();

            return farmType;
        }

        public async Task<BasePageList<FarmTypeResult>> GetAsync(FarmTypeFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var farmTypeQuery = _farmTypeRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                farmTypeQuery = farmTypeQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            if (filter.StatusId.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.StatusId == filter.StatusId);
            }

            if (filter.CreatedById.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                farmTypeQuery = farmTypeQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            var query = farmTypeQuery.Select(a => new FarmTypeResult
            {
                CreatedById = a.CreatedById,
                CreatedDate = a.CreatedDate,
                Description = a.Description,
                Id = a.Id,
                Name = a.Name,
                UpdatedById = a.UpdatedById,
                UpdatedDate = a.UpdatedDate,
                StatusId = a.StatusId
            });

            var filteredNumber = query.Select(x => x.Id).Count();

            var farmTypes = await query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize).ToListAsync();

            var result = new BasePageList<FarmTypeResult>(farmTypes)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<IList<FarmTypeResult>> SearchAsync(BaseFilter filter)
        {
            var keyword = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = _farmTypeRepository.Table
                .Select(c => new FarmTypeResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                });

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.ToLower().Contains(keyword) || x.Description.ToLower().Contains(keyword));
            }

            if (filter.PageSize > 0)
            {
                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            }

            var farmTypes = await query
                .Select(x => new FarmTypeResult()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return farmTypes;
        }

        public async Task<int> CreateAsync(FarmTypeModifyRequest farmType)
        {
            var newFarmType = new FarmType()
            {
                Name = farmType.Name,
                Description = farmType.Description,
                CreatedById = farmType.CreatedById,
                UpdatedById = farmType.UpdatedById,
                StatusId = FarmTypeStatus.Actived.GetCode()
            };
            newFarmType.UpdatedDate = DateTimeOffset.UtcNow;
            newFarmType.CreatedDate = DateTimeOffset.UtcNow;

            await _farmTypeRepository.InsertAsync(newFarmType);
            await _dbContext.SaveChangesAsync();
            return newFarmType.Id;
        }

        public async Task<bool> UpdateAsync(FarmTypeModifyRequest farmType)
        {
            await _farmTypeRepository.Get(x => x.Id == farmType.Id)
                .SetEntry(x => x.Description, farmType.Description)
                .SetEntry(x => x.Name, farmType.Name)
                .SetEntry(x => x.UpdatedById, farmType.UpdatedById)
                .SetEntry(x => x.UpdatedDate, DateTime.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(FarmTypeModifyRequest request)
        {
            await _farmTypeRepository.Get(x => x.Id == request.Id)
                .SetEntry(x => x.StatusId, (int)FarmTypeStatus.Inactived)
                .SetEntry(x => x.UpdatedById, request.UpdatedById)
                .SetEntry(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> ActiveAsync(FarmTypeModifyRequest request)
        {
            await _farmTypeRepository.Get(x => x.Id == request.Id)
                .SetEntry(x => x.StatusId, (int)FarmTypeStatus.Actived)
                .SetEntry(x => x.UpdatedById, request.UpdatedById)
                .SetEntry(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _farmTypeRepository.DeleteAsync(x => x.Id == id);
            await _dbContext.SaveChangesAsync();
            return deletedNumbers > 0;
        }
    }
}
