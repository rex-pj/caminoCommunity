using Camino.Core.Domains.Products;

namespace Camino.Core.Contracts.Repositories.Products
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory> FindAsync(int id);
        Task<int> CreateAsync(ProductCategory category);
        Task<bool> UpdateAsync(ProductCategory category);
        Task<ProductCategory> FindByNameAsync(string name);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasProductsAsync(int categoryId);
    }
}
