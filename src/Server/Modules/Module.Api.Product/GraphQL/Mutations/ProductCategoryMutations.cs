using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using Camino.Shared.General;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using Module.Api.Product.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Mutations
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
