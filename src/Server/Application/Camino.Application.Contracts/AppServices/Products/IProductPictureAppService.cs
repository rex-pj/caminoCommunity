using Camino.Application.Contracts.AppServices.Products.Dtos;
using Camino.Shared.Enums;

namespace Camino.Application.Contracts.AppServices.Products
{
    public interface IProductPictureAppService
    {
        Task<bool> CreateAsync(ProductPicturesModifyRequest request, bool needSaveChanges = false);
        Task<BasePageList<ProductPictureResult>> GetAsync(ProductPictureFilter filter);
        Task<IList<ProductPictureResult>> GetListByProductIdAsync(IdRequestFilter<long> filter, int? productPictureTypeId = null);
        Task<IList<ProductPictureResult>> GetListByProductIdsAsync(IEnumerable<long> productIds, IdRequestFilter<long> filter, ProductPictureTypes productPictureType);
        Task<bool> UpdateAsync(ProductPicturesModifyRequest request, bool needSaveChanges = false);
    }
}
