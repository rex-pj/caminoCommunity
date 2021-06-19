using Camino.Core.Domain.Identities;
using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using Module.Api.Product.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class ProductQueries : BaseQueries
    {
        public async Task<ProductPageListModel> GetUserProductsAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IProductResolver productResolver,
            ProductFilterModel criterias)
        {
            return await productResolver.GetUserProductsAsync(currentUser, criterias);
        }

        public async Task<ProductPageListModel> GetProductsAsync([Service] IProductResolver productResolver,
            ProductFilterModel criterias)
        {
            return await productResolver.GetProductsAsync(criterias);
        }

        public async Task<IList<ProductModel>> GetRelevantProductsAsync([Service] IProductResolver productResolver,
            ProductFilterModel criterias)
        {
            return await productResolver.GetRelevantProductsAsync(criterias);
        }

        public async Task<ProductModel> GetProductAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IProductResolver productResolver,
            ProductFilterModel criterias)
        {
            return await productResolver.GetProductAsync(currentUser, criterias);
        }
    }
}
