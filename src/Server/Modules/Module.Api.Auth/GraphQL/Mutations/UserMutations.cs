using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using Camino.Core.Domain.Identities;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Threading.Tasks;
using Camino.Shared.Requests.Authentication;
using System.Collections.Generic;
using Camino.Shared.General;

namespace Module.Api.Auth.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class UserMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserResolver userResolver, UpdatePerItemModel criterias)
        {
            return await userResolver.UpdateUserInfoItemAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserResolver userResolver, UserIdentifierUpdateModel criterias)
        {
            return await userResolver.UpdateIdentifierAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<UserTokenModel> UpdatePasswordAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserResolver userResolver, UserPasswordUpdateModel criterias)
        {
            return await userResolver.UpdatePasswordAsync(currentUser, criterias);
        }

        public async Task<CommonResult> SignupAsync([Service] IUserResolver userResolver, SignupModel criterias)
        {
            return await userResolver.SignupAsync(criterias);
        }

        public async Task<UserTokenModel> LoginAsync([Service] IUserResolver userResolver, LoginModel criterias)
        {
            return await userResolver.LoginAsync(criterias);
        }

        public async Task<CommonResult> ForgotPasswordAsync([Service] IUserResolver userResolver, ForgotPasswordModel criterias)
        {
            return await userResolver.ForgotPasswordAsync(criterias);
        }

        public async Task<CommonResult> ResetPasswordAsync([Service] IUserResolver userResolver, ResetPasswordModel criterias)
        {
            return await userResolver.ResetPasswordAsync(criterias);
        }

        public async Task<IEnumerable<SelectOption>> SelectUsersAsync([Service] IUserResolver userResolver, UserFilterModel criterias)
        {
            return await userResolver.SelectUsersAsync(criterias);
        }
    }
}
