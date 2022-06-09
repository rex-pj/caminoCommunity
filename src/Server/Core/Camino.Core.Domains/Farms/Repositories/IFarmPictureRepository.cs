using Camino.Shared.Enums;

namespace Camino.Core.Domains.Farms.Repositories
{
    public interface IFarmPictureRepository
    {
        Task<FarmPicture> GetByTypeAsync(long farmId, FarmPictureTypes pictureType);
        Task<long> CreateAsync(FarmPicture productPicture, bool needSaveChanges = false);
        Task UpdateAsync(FarmPicture productPicture, bool needSaveChanges = false);
    }
}
