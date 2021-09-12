using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Threading.Tasks;
using Camino.Shared.Requests.Authentication;
using System.Collections.Generic;
using Camino.Shared.General;
using System.Security.Claims;

namespace Module.Api.Auth.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class UserMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync(ClaimsPrincipal claimsPrincipal, [Service] IUserResolver userResolver, UpdatePerItemModel criterias)
        {
            return await userResolver.UpdateUserInfoItemAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(ClaimsPrincipal claimsPrincipal, [Service] IUserResolver userResolver, UserIdentifierUpdateModel criterias)
        {
            return await userResolver.UpdateIdentifierAsync(claimsPrincipal, criterias);
        }

        [GraphQlAuthentication]
        public async Task<UserTokenModel> UpdatePasswordAsync(ClaimsPrincipal claimsPrincipal, [Service] IAuthenticateResolver authenticateResolver, UserPasswordUpdateModel criterias)
        {
            return await authenticateResolver.UpdatePasswordAsync(claimsPrincipal, criterias);
        }

        public async Task<CommonResult> SignupAsync([Service] IUserResolver userResolver, SignupModel criterias)
        {
            return await userResolver.SignupAsync(criterias);
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

        public async Task<IEnumerable<SelectOption>> SelectUsersAsync([Service] IUserResolver userResolver, UserFilterModel criterias)
        {
            return await userResolver.SelectUsersAsync(criterias);
        }
    }
}
