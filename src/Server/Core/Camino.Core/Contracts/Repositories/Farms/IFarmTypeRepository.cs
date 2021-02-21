using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Requests.Farms;

namespace Camino.Core.Contracts.Repositories.Farms
{
    public interface IFarmTypeRepository
    {
        Task<FarmTypeResult> FindAsync(long id);
        FarmTypeResult FindByName(string name);
        Task<BasePageList<FarmTypeResult>> GetAsync(FarmTypeFilter filter);
        Task<IList<FarmTypeResult>> SearchAsync(string search = "", int page = 1, int pageSize = 10);
        Task<int> CreateAsync(FarmTypeModifyRequest farmType);
        Task<bool> UpdateAsync(FarmTypeModifyRequest farmType);
    }
}
