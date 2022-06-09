using Camino.Application.Contracts.AppServices.Products.Dtos;

namespace Camino.Application.Contracts.AppServices.Products
{
    public interface IProductAppService
    {
        Task<bool> ActiveAsync(long productId, long updatedById);
        Task<long> CreateAsync(ProductModifyRequest request);
        Task<bool> DeactiveAsync(long productId, long updatedById);
        Task<bool> DeleteAsync(long id);
        Task<ProductResult> FindAsync(IdRequestFilter<long> filter);
        Task<ProductResult> FindByNameAsync(string name);
        Task<ProductResult> FindDetailAsync(IdRequestFilter<long> filter);
        Task<BasePageList<ProductResult>> GetAsync(ProductFilter filter);
        Task<BasePageList<ProductPictureResult>> GetPicturesAsync(ProductPictureFilter filter);
        Task<IList<ProductResult>> GetProductByCategoryIdAsync(IdRequestFilter<int> categoryIdFilter);
        Task<IList<ProductResult>> GetRelevantsAsync(long id, ProductFilter filter);
        IList<SelectOption> SearchStatus(IdRequestFilter<int?> filter, string search = "");
        Task<bool> SoftDeleteAsync(long productId, long updatedById);
        Task<bool> UpdateAsync(ProductModifyRequest request);
    }
}