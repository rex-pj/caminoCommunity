using Module.Product.Api.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Product.Api.GraphQL.Resolvers.Contracts
{
    public interface IProductResolver
    {
        Task<ProductIdResultModel> CreateProductAsync(ClaimsPrincipal claimsPrincipal, CreateProductModel criterias);
        Task<ProductIdResultModel> UpdateProductAsync(ClaimsPrincipal claimsPrincipal, UpdateProductModel criterias);
        Task<ProductPageListModel> GetUserProductsAsync(ClaimsPrincipal claimsPrincipal, ProductFilterModel criterias);
        Task<ProductPageListModel> GetProductsAsync(ProductFilterModel criterias);
        Task<ProductModel> GetProductAsync(ClaimsPrincipal claimsPrincipal, ProductIdFilterModel criterias);
        Task<IList<ProductModel>> GetRelevantProductsAsync(ProductFilterModel criterias);
        Task<bool> DeleteProductAsync(ClaimsPrincipal claimsPrincipal, ProductIdFilterModel criterias);
    }
}
