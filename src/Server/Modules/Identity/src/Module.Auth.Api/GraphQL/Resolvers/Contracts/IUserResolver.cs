using Module.Auth.Api.Models;
using Camino.Infrastructure.AspNetCore.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Camino.Application.Contracts;

namespace Module.Auth.Api.GraphQL.Resolvers.Contracts
{
    public interface IUserResolver
    {
        Task<UserInfoModel> GetFullUserInfoAsync(ClaimsPrincipal claimsPrincipal, FindUserModel criterias);
        Task<UserPageListModel> GetUsersAsync(UserFilterModel criterias);
        Task<IEnumerable<SelectOption>> SelectUsersAsync(UserFilterModel criterias);
        Task<CommonResult> ActiveAsync(ActiveUserModel criterias);
    }
}
