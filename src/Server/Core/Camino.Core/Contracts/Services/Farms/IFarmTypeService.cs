using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Requests.Farms;
using Camino.Shared.General;

namespace Camino.Core.Contracts.Services.Farms
{
    public interface IFarmTypeService
    {
        Task<FarmTypeResult> FindAsync(long id);
        FarmTypeResult FindByName(string name);
        Task<BasePageList<FarmTypeResult>> GetAsync(FarmTypeFilter filter);
        Task<IList<FarmTypeResult>> SearchAsync(BaseFilter filter);
        Task<int> CreateAsync(FarmTypeModifyRequest farmType);
        Task<bool> UpdateAsync(FarmTypeModifyRequest farmType);
        Task<bool> DeactivateAsync(FarmTypeModifyRequest request);
        Task<bool> ActiveAsync(FarmTypeModifyRequest request);
        Task<bool> DeleteAsync(int id);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
    }
}
