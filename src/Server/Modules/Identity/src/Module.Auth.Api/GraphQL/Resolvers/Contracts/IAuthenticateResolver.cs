using Module.Auth.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Auth.Api.GraphQL.Resolvers.Contracts
{
    public interface IAuthenticateResolver
    {
        Task<UserInfoModel> GetLoggedUserAsync(ClaimsPrincipal claimsPrincipal);
    }
}
