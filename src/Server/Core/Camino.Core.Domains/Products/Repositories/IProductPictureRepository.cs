using Camino.Core.Domains.Products;
using Camino.Shared.Enums;

namespace Camino.Core.Contracts.Repositories.Products
{
    public interface IProductPictureRepository
    {
        Task<ProductPicture> GetByTypeAsync(long productId, ProductPictureTypes pictureType);
        Task<long> CreateAsync(ProductPicture productPicture, bool needSaveChanges = false);
        Task UpdateAsync(ProductPicture productPicture, bool needSaveChanges = false);
    }
}
