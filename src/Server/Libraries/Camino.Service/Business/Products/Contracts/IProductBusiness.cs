using Camino.Service.Projections.Filters;
using System.Threading.Tasks;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Product;
using System.Collections.Generic;

namespace Camino.Service.Business.Products.Contracts
{
    public interface IProductBusiness
    {
        Task<long> CreateAsync(ProductProjection product);
        Task<ProductProjection> FindAsync(long id);
        Task<ProductProjection> FindDetailAsync(long id);
        ProductProjection FindByName(string name);
        Task<ProductProjection> UpdateAsync(ProductProjection article);
        Task<BasePageList<ProductProjection>> GetAsync(ProductFilter filter);
        Task<IList<ProductProjection>> GetRelevantsAsync(long id, ProductFilter filter);
        Task<bool> DeleteAsync(long id);
    }
}