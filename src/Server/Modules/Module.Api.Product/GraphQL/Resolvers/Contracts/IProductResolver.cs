using Module.Api.Product.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Resolvers.Contracts
{
    public interface IProductResolver
    {
        Task<ProductModel> CreateProductAsync(ClaimsPrincipal claimsPrincipal, ProductModel criterias);
        Task<ProductModel> UpdateProductAsync(ClaimsPrincipal claimsPrincipal, ProductModel criterias);
        Task<ProductPageListModel> GetUserProductsAsync(ClaimsPrincipal claimsPrincipal, ProductFilterModel criterias);
        Task<ProductPageListModel> GetProductsAsync(ProductFilterModel criterias);
        Task<ProductModel> GetProductAsync(ClaimsPrincipal claimsPrincipal, ProductIdFilterModel criterias);
        Task<IList<ProductModel>> GetRelevantProductsAsync(ProductFilterModel criterias);
        Task<bool> DeleteProductAsync(ClaimsPrincipal claimsPrincipal, ProductIdFilterModel criterias);
    }
}
