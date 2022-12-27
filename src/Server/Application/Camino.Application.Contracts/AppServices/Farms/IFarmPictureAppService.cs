using Camino.Application.Contracts.AppServices.Farms.Dtos;
using Camino.Shared.Enums;

namespace Camino.Application.Contracts.AppServices.Farms
{
    public interface IFarmPictureAppService
    {
        Task<bool> CreateAsync(FarmPicturesModifyRequest request, bool needSaveChanges = false);
        Task<bool> DeleteByFarmIdAsync(long farmId);
        Task<BasePageList<FarmPictureResult>> GetAsync(FarmPictureFilter filter);
        Task<IList<FarmPictureResult>> GetListByFarmIdAsync(IdRequestFilter<long> filter, int? farmPictureTypeId = null);
        Task<IList<FarmPictureResult>> GetListByFarmIdsAsync(IEnumerable<long> farmIds, IdRequestFilter<long> filter, FarmPictureTypes farmPictureType);
        Task<bool> UpdateAsync(FarmPicturesModifyRequest request, bool needSaveChanges = false);
        Task<bool> UpdateStatusByFarmIdAsync(long farmId, long updatedById, PictureStatuses pictureStatus);
    }
}
