using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Auth.Api.GraphQL.Resolvers.Contracts;
using System.Security.Claims;
using System.Threading.Tasks;
using Module.Auth.Api.Models;

namespace Module.Auth.Api.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class AuthenticationQueries : BaseQueries
    {
        [GraphQlAuthentication]
        public async Task<UserInfoModel> GetLoggedUserAsync([Service] IAuthenticateResolver authenticateResolver, ClaimsPrincipal claimsPrincipal)
        {
            return await authenticateResolver.GetLoggedUserAsync(claimsPrincipal);
        }
    }
}
