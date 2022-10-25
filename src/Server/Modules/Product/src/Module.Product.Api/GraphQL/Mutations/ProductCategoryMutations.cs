using Camino.Application.Contracts;
using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Product.Api.GraphQL.Resolvers.Contracts;
using Module.Product.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Product.Api.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class ProductCategoryMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<IEnumerable<SelectOption>> GetProductCategoriesAsync([Service] IProductCategoryResolver productCategoryResolver,
            ProductCategorySelectFilterModel criterias)
        {
            return await productCategoryResolver.GetProductCategoriesAsync(criterias);
        }
    }
}
