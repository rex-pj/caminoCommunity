using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Order.GraphQL.Resolvers;
using Module.Api.Order.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Order.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class OrderMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<long> AddToCartAsync(ClaimsPrincipal claimsPrincipal, [Service] OrderResolver orderResolver, AddToCartModel criterias)
        {
            return await orderResolver.AddToCartAsync(claimsPrincipal, criterias);
        }
    }
}
