using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Mutations;
using Camino.Infrastructure.AspNetCore.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Auth.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class AuthenticationMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<UserTokenModel> UpdatePasswordAsync(ClaimsPrincipal claimsPrincipal, [Service] IAuthenticateResolver authenticateResolver, UserPasswordUpdateModel criterias)
        {
            return await authenticateResolver.UpdatePasswordAsync(claimsPrincipal, criterias);
        }

        public async Task<UserTokenModel> RefreshTokenAsync([Service] IAuthenticateResolver authenticateResolver)
        {
            return await authenticateResolver.RefreshTokenAsync();
        }

        public async Task<CommonResult> ForgotPasswordAsync([Service] IAuthenticateResolver authenticateResolver, ForgotPasswordModel criterias)
        {
            return await authenticateResolver.ForgotPasswordAsync(criterias);
        }

        public async Task<CommonResult> ResetPasswordAsync([Service] IAuthenticateResolver authenticateResolver, ResetPasswordModel criterias)
        {
            return await authenticateResolver.ResetPasswordAsync(criterias);
        }
    }
}
