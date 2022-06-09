using Camino.Core.Domains.Farms.Repositories;
using Camino.Core.Contracts.Repositories.Users;
using Camino.Application.Contracts.AppServices.Farms;
using Camino.Core.DependencyInjection;
using Camino.Application.Contracts;
using Camino.Shared.Enums;
using Camino.Application.Contracts.Utils;
using Camino.Application.Contracts.AppServices.Farms.Dtos;
using Camino.Core.Domains.Farms;
using Camino.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Camino.Shared.Utils;
using Camino.Shared.Exceptions;

namespace Camino.Application.AppServices.Farms
{
    public class FarmTypeAppService : IFarmTypeAppService, IScopedDependency
    {
        private readonly IFarmTypeRepository _farmTypeRepository;
        private readonly IEntityRepository<FarmType> _farmTypeEntityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFarmAppService _farmAppService;

        public FarmTypeAppService(IEntityRepository<FarmType> farmTypeEntityRepository,
            IFarmTypeRepository farmTypeRepository, IUserRepository userRepository,
            IFarmAppService farmAppService)
        {
            _farmTypeEntityRepository = farmTypeEntityRepository;
            _farmTypeRepository = farmTypeRepository;
            _userRepository = userRepository;
            _farmAppService = farmAppService;
        }

        #region get
        public async Task<FarmTypeResult> FindAsync(long id)
        {
            var exist = await _farmTypeRepository.FindAsync(id);
            if (exist == null)
            {
                return null;
            }

            var result = MapEntityToDto(exist);
            var createdByUser = await _userRepository.FindByIdAsync(exist.CreatedById);
            var updatedByUser = await _userRepository.FindByIdAsync(exist.UpdatedById);

            result.CreatedBy = createdByUser.DisplayName;
            result.UpdatedBy = updatedByUser.DisplayName;

            return result;
        }

        public async Task<FarmTypeResult> FindByNameAsync(string name)
        {
            var existing = await _farmTypeRepository.FindByNameAsync(name);
            return MapEntityToDto(existing);
        }

        public async Task<BasePageList<FarmTypeResult>> GetAsync(FarmTypeFilter filter)
        {
            var search = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var farmTypeQuery = _farmTypeEntityRepository.Table;
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

            await PopulateModifiersAsync(farmTypes);
            var result = new BasePageList<FarmTypeResult>(farmTypes)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        private async Task PopulateModifiersAsync(IList<FarmTypeResult> farmTypes)
        {
            var createdByIds = farmTypes.Select(x => x.CreatedById).ToArray();
            var updatedByIds = farmTypes.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetByIdsAsync(updatedByIds);

            foreach (var farmType in farmTypes)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == farmType.CreatedById);
                farmType.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == farmType.CreatedById);
                farmType.UpdatedBy = updatedBy.DisplayName;
            }
        }

        public async Task<IList<FarmTypeResult>> SearchAsync(BaseFilter filter)
        {
            var keyword = filter.Keyword != null ? filter.Keyword.ToLower() : "";
            var query = _farmTypeEntityRepository.Table
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

        private FarmTypeResult MapEntityToDto(FarmType entity)
        {
            return new FarmTypeResult
            {
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                CreatedById = entity.CreatedById,
                Id = entity.Id,
                Name = entity.Name,
                UpdatedById = entity.UpdatedById,
                UpdatedDate = entity.UpdatedDate,
                StatusId = entity.StatusId
            };
        }
        #endregion

        #region CRUD
        public async Task<int> CreateAsync(FarmTypeModifyRequest farmType)
        {
            var newFarmType = new FarmType()
            {
                Name = farmType.Name,
                Description = farmType.Description,
                CreatedById = farmType.CreatedById,
                UpdatedById = farmType.UpdatedById,
                StatusId = FarmTypeStatuses.Actived.GetCode()
            };
            return await _farmTypeRepository.CreateAsync(newFarmType);
        }

        public async Task<bool> UpdateAsync(FarmTypeModifyRequest farmType)
        {
            var existing = await _farmTypeRepository.FindAsync(farmType.Id);
            existing.Description = farmType.Description;
            existing.Name = farmType.Name;
            existing.UpdatedById = farmType.UpdatedById;
            return await _farmTypeRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeactivateAsync(FarmTypeModifyRequest farmType)
        {
            var existing = await _farmTypeRepository.FindAsync(farmType.Id);
            existing.StatusId = (int)FarmTypeStatuses.Inactived;
            existing.UpdatedById = farmType.UpdatedById;
            return await _farmTypeRepository.UpdateAsync(existing);
        }

        public async Task<bool> ActiveAsync(FarmTypeModifyRequest farmType)
        {
            var existing = await _farmTypeRepository.FindAsync(farmType.Id);
            existing.StatusId = (int)FarmTypeStatuses.Actived;
            existing.UpdatedById = farmType.UpdatedById;
            return await _farmTypeRepository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var farms = await _farmAppService.GetByTypeAsync(new IdRequestFilter<long>
            {
                Id = id,
                CanGetDeleted = true,
                CanGetInactived = true
            });

            if (farms.Any())
            {
                throw new CaminoApplicationException($"Some {nameof(farms)} belong to this farm type need to be deleted or move to another farm type");
            }

            return await _farmTypeRepository.DeleteAsync(id);
        }
        #endregion

        #region category status
        public IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "")
        {
            search = search != null ? search.ToLower() : "";
            var result = new List<SelectOption>();
            if (filter.Id.HasValue)
            {
                var selected = (FarmTypeStatuses)filter.Id;
                result = SelectOptionUtils.ToSelectOptions(selected).ToList();
            }
            else
            {
                result = SelectOptionUtils.ToSelectOptions<FarmTypeStatuses>().ToList();
            }

            result = result.Where(x => string.IsNullOrEmpty(search) || x.Text.ToLower().Equals(search)).ToList();
            return result;
        }
        #endregion
    }
}
