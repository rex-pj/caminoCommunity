using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using Camino.IdentityManager.Models;
using Camino.Service.Projections.Request;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Threading.Tasks;

namespace Module.Api.Auth.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class UserMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserResolver userResolver, UpdatePerItemModel criterias)
        {
            return await userResolver.UpdateUserInfoItemAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserResolver userResolver, UserIdentifierUpdateRequest criterias)
        {
            return await userResolver.UpdateIdentifierAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<UserTokenModel> UpdatePasswordAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserResolver userResolver, UserPasswordUpdateRequest criterias)
        {
            return await userResolver.UpdatePasswordAsync(currentUser, criterias);
        }

        public async Task<CommonResult> SignupAsync([Service] IUserResolver userResolver, SignupModel criterias)
        {
            return await userResolver.SignupAsync(criterias);
        }

        public async Task<UserTokenModel> SigninAsync([Service] IUserResolver userResolver, SigninModel criterias)
        {
            return await userResolver.SigninAsync(criterias);
        }

        public async Task<CommonResult> ForgotPasswordAsync([Service] IUserResolver userResolver, ForgotPasswordModel criterias)
        {
            return await userResolver.ForgotPasswordAsync(criterias);
        }

        public async Task<CommonResult> ResetPasswordAsync([Service] IUserResolver userResolver, ResetPasswordModel criterias)
        {
            return await userResolver.ResetPasswordAsync(criterias);
        }
    }
}
