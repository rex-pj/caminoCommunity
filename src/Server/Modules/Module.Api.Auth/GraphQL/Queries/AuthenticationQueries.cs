using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Queries;
using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Auth.GraphQL.Queries
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
