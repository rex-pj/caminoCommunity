using Camino.Service.Projections.Farm;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using System.Threading.Tasks;

namespace Camino.Service.Business.Farms.Contracts
{
    public interface IFarmPictureBusiness
    {
        Task<BasePageList<FarmPictureProjection>> GetAsync(FarmPictureFilter filter);
    }
}
