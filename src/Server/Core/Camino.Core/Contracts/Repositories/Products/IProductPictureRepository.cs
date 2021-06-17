using Camino.Shared.Enums;
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
        Task<bool> UpdateStatusByProductIdAsync(ProductPicturesModifyRequest request, PictureStatus pictureStatus);
        Task<bool> DeleteByProductIdsAsync(IEnumerable<long> ids);
        Task<bool> UpdateStatusByProductIdsAsync(IEnumerable<long> ids, long updatedById, PictureStatus status);
        Task<bool> CreateAsync(ProductPicturesModifyRequest request);
        Task<bool> UpdateAsync(ProductPicturesModifyRequest request);
        Task<IList<ProductPictureResult>> GetProductPicturesByProductIdAsync(IdRequestFilter<long> filter, int? productPictureTypeId = null);
        Task<IList<ProductPictureResult>> GetProductPicturesByProductIdsAsync(IEnumerable<long> productIds, int productPictureTypeId);
    }
}
