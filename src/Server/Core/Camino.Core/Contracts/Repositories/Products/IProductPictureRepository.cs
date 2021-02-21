using Camino.Shared.Requests.Filters;
using Camino.Shared.Requests.Products;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Repositories.Products
{
    public interface IProductPictureRepository
    {
        Task<BasePageList<ProductPictureResult>> GetAsync(ProductPictureFilter filter);
        Task<bool> DeleteByProductIdAsync(long id);
        Task<bool> SoftDeleteByProductIdAsync(long id);
        Task<bool> DeleteByProductIdsAsync(IEnumerable<long> ids);
        Task<bool> SoftDeleteByProductIdsAsync(IEnumerable<long> ids);
        Task<bool> CreateAsync(ProductPicturesModifyRequest request);
        Task<bool> UpdateAsync(ProductPicturesModifyRequest request);
        Task<IList<ProductPictureResult>> GetProductPicturesByProductIdAsync(long productId, int? productPictureTypeId = null);
        Task<IList<ProductPictureResult>> GetProductPicturesByProductIdsAsync(IEnumerable<long> productIds, int productPictureTypeId);
    }
}
