using Camino.Shared.Requests.Filters;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using System.Collections.Generic;
using Camino.Shared.Requests.Products;
using Camino.Shared.General;

namespace Camino.Core.Contracts.Services.Products
{
    public interface IProductService
    {
        Task<long> CreateAsync(ProductModifyRequest request);
        Task<ProductResult> FindAsync(IdRequestFilter<long> filter);
        Task<ProductResult> FindDetailAsync(IdRequestFilter<long> filter);
        ProductResult FindByName(string name);
        Task<ProductAttributeRelationResult> GetAttributeRelationByIdAsync(long id);
        Task<ProductAttributeRelationValueResult> GetAttributeRelationValueByIdAsync(long id);
        Task<bool> UpdateAsync(ProductModifyRequest request);
        Task<BasePageList<ProductResult>> GetAsync(ProductFilter filter);
        Task<IList<ProductResult>> GetRelevantsAsync(long id, ProductFilter filter);
        Task<BasePageList<ProductPictureResult>> GetPicturesAsync(ProductPictureFilter filter);
        Task<bool> DeleteAsync(long id);
        Task<bool> SoftDeleteAsync(ProductModifyRequest request);
        Task<bool> DeactiveAsync(ProductModifyRequest request);
        Task<bool> ActiveAsync(ProductModifyRequest request);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
    }
}