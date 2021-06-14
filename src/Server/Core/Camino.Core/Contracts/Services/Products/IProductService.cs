using Camino.Shared.Requests.Filters;
using System.Threading.Tasks;
using Camino.Shared.Results.PageList;
using Camino.Shared.Results.Products;
using System.Collections.Generic;
using Camino.Shared.Requests.Products;

namespace Camino.Core.Contracts.Services.Products
{
    public interface IProductService
    {
        Task<long> CreateAsync(ProductModifyRequest request);
        Task<ProductResult> FindAsync(long id);
        Task<ProductResult> FindDetailAsync(long id);
        ProductResult FindByName(string name);
        Task<bool> UpdateAsync(ProductModifyRequest request);
        Task<BasePageList<ProductResult>> GetAsync(ProductFilter filter);
        Task<IList<ProductResult>> GetRelevantsAsync(long id, ProductFilter filter);
        Task<bool> DeleteAsync(long id);
        Task<bool> SoftDeleteAsync(long id);
        Task<bool> DeactivateAsync(long id);
        Task<BasePageList<ProductPictureResult>> GetPicturesAsync(ProductPictureFilter filter);
        Task<bool> ActiveAsync(long id);
    }
}