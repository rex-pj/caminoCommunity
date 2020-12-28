using Camino.IdentityManager.Models;
using Module.Api.Product.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Resolvers.Contracts
{
    public interface IProductResolver
    {
        Task<ProductModel> CreateProductAsync(ApplicationUser currentUser, ProductModel criterias);
        Task<ProductModel> UpdateProductAsync(ApplicationUser currentUser, ProductModel criterias);
        Task<ProductPageListModel> GetUserProductsAsync(ProductFilterModel criterias);
        Task<ProductPageListModel> GetProductsAsync(ProductFilterModel criterias);
        Task<ProductModel> GetProductAsync(ProductFilterModel criterias);
        Task<IList<ProductModel>> GetRelevantProductsAsync(ProductFilterModel criterias);
        Task<bool> DeleteProductAsync(ApplicationUser currentUser, ProductFilterModel criterias);
    }
}
