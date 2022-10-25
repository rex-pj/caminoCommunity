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
    public class ProductAttributeMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<IEnumerable<SelectOption>> GetProductAttributesAsync([Service] IProductAttributeResolver productAttributeResolver,
            AttributeSelectFilterModel criterias)
        {
            return await productAttributeResolver.GetProductAttributesAsync(criterias);
        }

        [GraphQlAuthentication]
        public IEnumerable<SelectOption> GetProductAttributeControlTypes([Service] IProductAttributeResolver productAttributeResolver,
            AttributeControlTypeSelectFilterModel criterias)
        {
            return productAttributeResolver.GetProductAttributeControlTypes(criterias);
        }
    }
}
