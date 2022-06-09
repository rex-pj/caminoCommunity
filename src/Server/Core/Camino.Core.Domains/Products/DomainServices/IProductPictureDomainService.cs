using Camino.Shared.Enums;

namespace Camino.Core.Domains.Products.DomainServices
{
    public interface IProductPictureDomainService
    {
        Task DeleteByProductIdAsync(long productId, bool needSaveChanges = false);
        Task DeleteByProductIdAsync(long productId, IList<long> pictureIds, bool needSaveChanges = false);
        Task<bool> DeleteByProductIdsAsync(IEnumerable<long> ids, bool needSaveChanges = false);
        Task<bool> UpdateStatusByProductIdAsync(long productId, long updatedById, PictureStatuses pictureStatus);
        Task<bool> UpdateStatusByProductIdsAsync(IEnumerable<long> productIds, long updatedById, PictureStatuses pictureStatus);
    }
}
