using Camino.Core.Domains.Products;

namespace Camino.Core.Contracts.Repositories.Products
{
    public interface IProductRepository
    {
        Task<long> CreateAsync(Product product);
        Task<Product> FindAsync(long id);
        Task<Product> FindByNameAsync(string name);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(long id);
    }
}