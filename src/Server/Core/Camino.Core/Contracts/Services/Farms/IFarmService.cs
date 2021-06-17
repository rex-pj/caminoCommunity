using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Shared.Requests.Farms;

namespace Camino.Core.Contracts.Services.Farms
{
    public interface IFarmService
    {
        Task<FarmResult> FindAsync(IdRequestFilter<long> filter);
        Task<FarmResult> FindDetailAsync(IdRequestFilter<long> filter);
        FarmResult FindByName(string name);
        Task<BasePageList<FarmResult>> GetAsync(FarmFilter filter);
        Task<long> CreateAsync(FarmModifyRequest request);
        Task<bool> UpdateAsync(FarmModifyRequest request);
        Task<IList<FarmResult>> SelectAsync(SelectFilter filter, int page = 1, int pageSize = 10);
        Task<bool> DeleteAsync(long id);
        Task<bool> SoftDeleteAsync(FarmModifyRequest request);
        Task<bool> DeactivateAsync(FarmModifyRequest request);
        Task<BasePageList<FarmPictureResult>> GetPicturesAsync(FarmPictureFilter filter);
        Task<bool> ActivateAsync(FarmModifyRequest request);
    }
}
