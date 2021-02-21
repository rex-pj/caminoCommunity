using Camino.Shared.Requests.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Farms;
using Camino.Core.Contracts.Services.Farms;
using Camino.Core.Domain.Farms;
using Camino.Shared.Requests.Farms;
using Camino.Core.Contracts.Repositories.Farms;
using Camino.Core.Contracts.Repositories.Users;
using System.Linq;

namespace Camino.Services.Farms
{
    public class FarmTypeService : IFarmTypeService
    {
        private readonly IFarmTypeRepository _farmTypeRepository;
        private readonly IUserRepository _userRepository;

        public FarmTypeService(IFarmTypeRepository farmTypeRepository, IUserRepository userRepository)
        {
            _farmTypeRepository = farmTypeRepository;
            _userRepository = userRepository;
        }

        public async Task<FarmTypeResult> FindAsync(long id)
        {
            var exist = await _farmTypeRepository.FindAsync(id);
            if (exist == null)
            {
                return null;
            }

            var createdByUser = await _userRepository.FindByIdAsync(exist.CreatedById);
            var updatedByUser = await _userRepository.FindByIdAsync(exist.UpdatedById);

            exist.CreatedBy = createdByUser.DisplayName;
            exist.UpdatedBy = updatedByUser.DisplayName;

            return exist;
        }

        public FarmTypeResult FindByName(string name)
        {
            return _farmTypeRepository.FindByName(name);
        }

        public async Task<BasePageList<FarmTypeResult>> GetAsync(FarmTypeFilter filter)
        {
            var farmTypesPageList = await _farmTypeRepository.GetAsync(filter);
            var createdByIds = farmTypesPageList.Collections.Select(x => x.CreatedById).ToArray();
            var updatedByIds = farmTypesPageList.Collections.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = await _userRepository.GetNameByIdsAsync(createdByIds);
            var updatedByUsers = await _userRepository.GetNameByIdsAsync(updatedByIds);

            foreach (var farmType in farmTypesPageList.Collections)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == farmType.CreatedById);
                farmType.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == farmType.CreatedById);
                farmType.UpdatedBy = updatedBy.DisplayName;
            }
            return farmTypesPageList;
        }

        public async Task<IList<FarmTypeResult>> SearchAsync(string search = "", int page = 1, int pageSize = 10)
        {
            return await _farmTypeRepository.SearchAsync(search, page, pageSize);
        }

        public async Task<int> CreateAsync(FarmTypeModifyRequest farmType)
        {
            return await _farmTypeRepository.CreateAsync(farmType);
        }

        public async Task<bool> UpdateAsync(FarmTypeModifyRequest farmType)
        {
            return await _farmTypeRepository.UpdateAsync(farmType);
        }
    }
}
