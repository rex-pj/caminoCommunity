using Camino.Core.Contracts.Data;
using Camino.Shared.Requests.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Farms;
using Camino.Core.Contracts.Repositories.Farms;
using Camino.Core.Domain.Farms;
using Camino.Shared.Requests.Farms;
using Camino.Shared.Enums;
using Camino.Core.Utils;

namespace Camino.Infrastructure.Repositories.Farms
{
    public class FarmTypeRepository : IFarmTypeRepository
    {
        private readonly IRepository<FarmType> _farmTypeRepository;

        public FarmTypeRepository(IRepository<FarmType> farmTypeRepository)
        {
            _farmTypeRepository = farmTypeRepository;
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
            var search = filter.Search != null ? filter.Search.ToLower() : "";
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

        public async Task<IList<FarmTypeResult>> SearchAsync(string search = "", int page = 1, int pageSize = 10)
        {
            if (search == null)
            {
                search = string.Empty;
            }

            search = search.ToLower();
            var query = _farmTypeRepository.Table
                .Select(c => new FarmTypeResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                });

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search) || x.Description.ToLower().Contains(search));
            }

            if (pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
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

            var id = await _farmTypeRepository.AddWithInt32EntityAsync(newFarmType);
            return id;
        }

        public async Task<bool> UpdateAsync(FarmTypeModifyRequest farmType)
        {
            var exist = await _farmTypeRepository.Get(x => x.Id == farmType.Id)
                .Set(x => x.Description, farmType.Description)
                .Set(x => x.Name, farmType.Name)
                .Set(x => x.UpdatedById, farmType.UpdatedById)
                .Set(x => x.UpdatedDate, DateTime.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(FarmTypeModifyRequest request)
        {
            await _farmTypeRepository.Get(x => x.Id == request.Id)
                .Set(x => x.StatusId, (int)FarmTypeStatus.Inactived)
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> ActiveAsync(FarmTypeModifyRequest request)
        {
            await _farmTypeRepository.Get(x => x.Id == request.Id)
                .Set(x => x.StatusId, (int)FarmTypeStatus.Actived)
                .Set(x => x.UpdatedById, request.UpdatedById)
                .Set(x => x.UpdatedDate, DateTimeOffset.UtcNow)
                .UpdateAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedNumbers = await _farmTypeRepository.Get(x => x.Id == id).DeleteAsync();
            return deletedNumbers > 0;
        }
    }
}
