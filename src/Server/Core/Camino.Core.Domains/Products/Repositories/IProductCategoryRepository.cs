using Camino.Core.Domains.Products;

namespace Camino.Core.Contracts.Repositories.Products
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory> FindAsync(long id);
        Task<long> CreateAsync(ProductCategory category);
        Task<bool> UpdateAsync(ProductCategory category);
        Task<ProductCategory> FindByNameAsync(string name);
        Task<bool> DeleteAsync(long id);
        Task<bool> HasProductsAsync(long categoryId);
    }
}
