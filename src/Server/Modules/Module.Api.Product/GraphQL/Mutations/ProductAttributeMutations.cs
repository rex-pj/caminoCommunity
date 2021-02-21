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
    public class ProductAttributeMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<IEnumerable<SelectOption>> GetProductAttributesAsync([Service] IProductAttributeResolver productAttributeResolver,
            BaseSelectFilterModel criterias)
        {
            return await productAttributeResolver.GetProductAttributesAsync(criterias);
        }

        [GraphQlAuthentication]
        public IEnumerable<SelectOption> GetProductAttributeControlTypes([Service] IProductAttributeResolver productAttributeResolver,
            ProductAttributeControlTypeSelectFilterModel criterias)
        {
            return productAttributeResolver.GetProductAttributeControlTypes(criterias);
        }
    }
}
