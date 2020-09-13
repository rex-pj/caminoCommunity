using Camino.Service.Projections.Farm;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
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
