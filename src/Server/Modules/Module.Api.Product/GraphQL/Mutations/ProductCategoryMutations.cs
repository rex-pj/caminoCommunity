using Camino.Core.Models;
using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class ProductCategoryMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<IEnumerable<SelectOption>> GetProductCategoriesAsync([Service] IProductCategoryResolver productCategoryResolver,
            SelectFilterModel criterias)
        {
            return await productCategoryResolver.GetProductCategoriesAsync(criterias);
        }
    }
}
