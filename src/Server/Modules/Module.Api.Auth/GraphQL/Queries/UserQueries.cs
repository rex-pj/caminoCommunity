using Camino.Infrastructure.AspNetCore.Models;
using Camino.Infrastructure.GraphQL.Attributes;
using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Auth.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class UserQueries : BaseQueries
    {
        [GraphQlAuthentication]
        public async Task<UserPageListModel> GetUsersAsync([Service] IUserResolver userResolver, UserFilterModel criterias)
        {
            return await userResolver.GetUsersAsync(criterias);
        }

        public async Task<UserInfoModel> GetFullUserInfoAsync(ClaimsPrincipal claimsPrincipal, [Service] IUserResolver userResolver, FindUserModel criterias)
        {
            return await userResolver.GetFullUserInfoAsync(claimsPrincipal, criterias);
        }

        public async Task<CommonResult> ActiveAsync([Service] IUserResolver userResolver, ActiveUserModel criterias)
        {
            return (await userResolver.ActiveAsync(criterias));
        }
    }
}
