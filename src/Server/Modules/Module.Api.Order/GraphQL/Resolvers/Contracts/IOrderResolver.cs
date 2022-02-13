using Module.Api.Order.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Order.GraphQL.Resolvers.Contracts
{
    public interface IOrderResolver
    {
        Task<long> AddToCartAsync(ClaimsPrincipal claimsPrincipal, AddToCartModel request);
    }
}
