using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Queries;
using Camino.Framework.Models;
using Camino.IdentityManager.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Threading.Tasks;

namespace Module.Api.Auth.GraphQL.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class UserQueries : BaseQueries
    {
        [GraphQlAuthentication]
        public FullUserInfoModel GetLoggedUser([Service] IUserResolver userResolver, [ApplicationUserState] ApplicationUser currentUser)
        {
            return userResolver.GetLoggedUser(currentUser);
        }

        [GraphQlAuthentication]
        public async Task<CommonResult> LogoutAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserResolver userResolver)
        {
            return await userResolver.LogoutAsync(currentUser);
        }
        public async Task<FullUserInfoModel> GetFullUserInfoAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IUserResolver userResolver, FindUserModel criterias)
        {
            return await userResolver.GetFullUserInfoAsync(currentUser, criterias);
        }

        public async Task<CommonResult> ActiveAsync([Service] IUserResolver userResolver, ActiveUserModel criterias)
        {
            return (await userResolver.ActiveAsync(criterias));
        }
    }
}
