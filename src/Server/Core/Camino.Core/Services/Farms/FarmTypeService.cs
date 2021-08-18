using Camino.Shared.Requests.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Farms;
using Camino.Core.Contracts.Services.Farms;
using Camino.Shared.Requests.Farms;
using Camino.Core.Contracts.Repositories.Farms;
using Camino.Core.Contracts.Repositories.Users;
using System.Linq;
using Camino.Core.Exceptions;
using Camino.Shared.General;

namespace Camino.Services.Farms
{
    public class FarmTypeService : IFarmTypeService
    {
        private readonly IFarmTypeRepository _farmTypeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFarmRepository _farmRepository;
        private readonly IFarmTypeStatusRepository _farmTypeStatusRepository;

        public FarmTypeService(IFarmTypeRepository farmTypeRepository, IUserRepository userRepository,
            IFarmRepository farmRepository, IFarmTypeStatusRepository farmTypeStatusRepository)
        {
            _farmTypeRepository = farmTypeRepository;
            _userRepository = userRepository;
            _farmRepository = farmRepository;
            _farmTypeStatusRepository = farmTypeStatusRepository;
        }

        #region get
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

        public async Task<IList<FarmTypeResult>> SearchAsync(BaseFilter filter)
        {
            return await _farmTypeRepository.SearchAsync(filter);
        }
        #endregion

        #region CRUD
        public async Task<int> CreateAsync(FarmTypeModifyRequest farmType)
        {
            return await _farmTypeRepository.CreateAsync(farmType);
        }

        public async Task<bool> UpdateAsync(FarmTypeModifyRequest farmType)
        {
            return await _farmTypeRepository.UpdateAsync(farmType);
        }

        public async Task<bool> DeactivateAsync(FarmTypeModifyRequest farmType)
        {
            return await _farmTypeRepository.DeactivateAsync(farmType);
        }

        public async Task<bool> ActiveAsync(FarmTypeModifyRequest farmType)
        {
            return await _farmTypeRepository.ActiveAsync(farmType);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var farms = await _farmRepository.GetFarmByTypeIdAsync(new IdRequestFilter<int>
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
            return _farmTypeStatusRepository.Search(filter, search);
        }
        #endregion
    }
}
