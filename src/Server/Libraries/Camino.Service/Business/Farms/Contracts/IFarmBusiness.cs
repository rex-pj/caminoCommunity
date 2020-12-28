using Camino.Service.Projections.Farm;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Service.Business.Farms.Contracts
{
    public interface IFarmBusiness
    {
        Task<FarmProjection> FindAsync(long id);
        Task<FarmProjection> FindDetailAsync(long id);
        FarmProjection FindByName(string name);
        Task<BasePageList<FarmProjection>> GetAsync(FarmFilter filter);
        Task<long> CreateAsync(FarmProjection farm);
        Task<FarmProjection> UpdateAsync(FarmProjection farm);
        Task<IList<FarmProjection>> SearchByUserIdAsync(long userId, string search = "", int page = 1, int pageSize = 10);
        Task<bool> DeleteAsync(long id);
    }
}
