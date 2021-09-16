using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using Module.Api.Product.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class ProductMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<ProductModel> CreateProductAsync(ClaimsPrincipal claimsPrincipal, [Service] IProductResolver productResolver,
            ProductModel criterias)
        {
            return await productResolver.CreateProductAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<ProductModel> UpdateProductAsync(ClaimsPrincipal claimsPrincipal, [Service] IProductResolver productResolver,
            ProductModel criterias)
        {
            return await productResolver.UpdateProductAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<bool> DeleteProductAsync(ClaimsPrincipal claimsPrincipal, [Service] IProductResolver productResolver,
            ProductIdFilterModel criterias)
        {
            return await productResolver.DeleteProductAsync(claimsPrincipal, criterias);
        }
    }
}
