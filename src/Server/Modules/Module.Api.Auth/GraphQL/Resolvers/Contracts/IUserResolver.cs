using Module.Api.Auth.Models;
using Camino.Framework.Models;
using System.Threading.Tasks;
using Camino.Shared.Requests.Authentication;
using System.Collections.Generic;
using Camino.Shared.General;
using System.Security.Claims;

namespace Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface IUserResolver
    {
        Task<UserInfoModel> GetLoggedUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<UserInfoModel> GetFullUserInfoAsync(ClaimsPrincipal claimsPrincipal, FindUserModel criterias);
        Task<UpdatePerItemModel> UpdateUserInfoItemAsync(ClaimsPrincipal claimsPrincipal, UpdatePerItemModel criterias);
        Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(ClaimsPrincipal claimsPrincipal, UserIdentifierUpdateModel criterias);
        Task<UserPageListModel> GetUsersAsync(UserFilterModel criterias);
        Task<IEnumerable<SelectOption>> SelectUsersAsync(UserFilterModel criterias);
        Task<CommonResult> SignupAsync(SignupModel criterias);
        Task<CommonResult> ActiveAsync(ActiveUserModel criterias);
    }
}
