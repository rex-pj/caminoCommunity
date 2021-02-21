using Module.Api.Auth.Models;
using Camino.Framework.Models;
using System.Threading.Tasks;
using Camino.Core.Domain.Identities;
using Camino.Shared.Requests.Authentication;

namespace Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface IUserResolver
    {
        UserInfoModel GetLoggedUser(ApplicationUser currentUser);
        Task<UserInfoModel> GetFullUserInfoAsync(ApplicationUser currentUser, FindUserModel criterias);
        Task<UpdatePerItemModel> UpdateUserInfoItemAsync(ApplicationUser currentUser, UpdatePerItemModel criterias);
        Task<CommonResult> LogoutAsync(ApplicationUser currentUser);
        Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(ApplicationUser currentUser, UserIdentifierUpdateRequest criterias);
        Task<UserTokenModel> UpdatePasswordAsync(ApplicationUser currentUser, UserPasswordUpdateRequest criterias);
        Task<UserPageListModel> GetUsersAsync(UserFilterModel criterias);
        Task<UserTokenModel> LoginAsync(LoginModel criterias);
        Task<CommonResult> SignupAsync(SignupModel criterias);
        Task<CommonResult> ActiveAsync(ActiveUserModel criterias);
        Task<CommonResult> ForgotPasswordAsync(ForgotPasswordModel criterias);
        Task<CommonResult> ResetPasswordAsync(ResetPasswordModel criterias);
    }
}
