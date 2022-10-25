using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Product.Api.GraphQL.Resolvers.Contracts;
using Module.Product.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Product.Api.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class ProductMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<ProductIdResultModel> CreateProductAsync(ClaimsPrincipal claimsPrincipal, [Service] IProductResolver productResolver,
            CreateProductModel criterias)
        {
            return await productResolver.CreateProductAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<ProductIdResultModel> UpdateProductAsync(ClaimsPrincipal claimsPrincipal, [Service] IProductResolver productResolver,
            UpdateProductModel criterias)
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
