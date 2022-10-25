using Module.Order.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Order.Api.GraphQL.Resolvers.Contracts
{
    public interface IOrderResolver
    {
        Task<long> AddToCartAsync(ClaimsPrincipal claimsPrincipal, AddToCartModel request);
    }
}
