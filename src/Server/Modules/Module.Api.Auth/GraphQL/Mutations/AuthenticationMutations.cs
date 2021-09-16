using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
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

        public async Task<UserTokenModel> LoginAsync([Service] IAuthenticateResolver authenticateResolver, LoginModel criterias)
        {
            return await authenticateResolver.LoginAsync(criterias);
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
