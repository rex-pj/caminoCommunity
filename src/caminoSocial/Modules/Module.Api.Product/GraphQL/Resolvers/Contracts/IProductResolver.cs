using Camino.Service.Business.Products.Contracts;
using HotChocolate;
using Module.Api.Product.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Resolvers.Contracts
{
    public interface IProductResolver
    {
        Task<ProductModel> CreateProductAsync(ProductModel criterias, [Service] IProductBusiness productBusiness);
        Task<ProductPageListModel> GetUserProductsAsync(ProductFilterModel criterias, [Service] IProductBusiness productBusiness);
        Task<ProductPageListModel> GetProductsAsync(ProductFilterModel criterias, [Service] IProductBusiness productBusiness);
        Task<ProductModel> GetProductAsync(ProductFilterModel criterias, [Service] IProductBusiness productBusiness);
        Task<IList<ProductModel>> GetRelevantProductsAsync(ProductFilterModel criterias, [Service] IProductBusiness productBusiness);
    }
}
