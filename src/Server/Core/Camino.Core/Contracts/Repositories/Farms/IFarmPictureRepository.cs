using Camino.Shared.Results.Farms;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Results.PageList;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Shared.Requests.Farms;
using Camino.Shared.Enums;

namespace Camino.Core.Contracts.Repositories.Farms
{
    public interface IFarmPictureRepository
    {
        Task<BasePageList<FarmPictureResult>> GetAsync(FarmPictureFilter filter);
        Task<IList<FarmPictureResult>> GetFarmPicturesByFarmIdAsync(long farmId, int? farmPictureTypeId = null);
        Task<IList<FarmPictureResult>> GetFarmPicturesByFarmIdsAsync(IEnumerable<long> farmIds, int farmPictureTypeId);
        Task<bool> CreateAsync(FarmPicturesModifyRequest request);
        Task<bool> UpdateAsync(FarmPicturesModifyRequest request);
        Task<bool> DeleteByFarmIdAsync(long farmId);
        Task<bool> UpdateStatusByFarmIdAsync(FarmPicturesModifyRequest request, PictureStatus pictureStatus);
    }
}
