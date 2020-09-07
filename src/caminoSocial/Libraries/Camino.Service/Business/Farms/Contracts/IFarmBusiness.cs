using Camino.Service.Data.Farm;
using Camino.Service.Data.Filters;
using Camino.Service.Data.PageList;
using System.Threading.Tasks;

namespace Camino.Service.Business.Farms.Contracts
{
    public interface IFarmBusiness
    {
        FarmProjection Find(long id);
        FarmProjection FindDetail(long id);
        FarmProjection FindByName(string name);
        Task<BasePageList<FarmProjection>> GetAsync(FarmFilter filter);
        int Add(FarmProjection farm);
        Task<FarmProjection> UpdateAsync(FarmProjection farm);
    }
}
