using Module.Api.Product.Models;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Resolvers.Contracts
{
    public interface IProductResolver
    {
        Task<ProductModel> CreateProductAsync(ProductModel criterias);
        Task<ProductPageListModel> GetUserProductsAsync(ProductFilterModel criterias);
    }
}
