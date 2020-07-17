using Api.Auth.Models;
using Coco.Auth.Models;
using Coco.Core.Dtos.Identity;
using Coco.Framework.Models;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IUserResolver
    {
        FullUserInfoModel GetLoggedUser();
        Task<FullUserInfoModel> GetFullUserInfoAsync(FindUserModel criterias);
        Task<UpdatePerItemModel> UpdateUserInfoItemAsync(UpdatePerItemModel criterias);
        Task<ICommonResult> SignoutAsync();
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(UserIdentifierUpdateDto criterias);
        Task<UserTokenResult> UpdatePasswordAsync(UserPasswordUpdateDto criterias);

        Task<UserTokenResult> SigninAsync(SigninModel criterias);
        Task<ICommonResult> SignupAsync(SignupModel criterias);
        Task<ICommonResult> ActiveAsync(ActiveUserModel criterias);
        Task<ICommonResult> ForgotPasswordAsync(ForgotPasswordModel criterias);
        Task<ICommonResult> ResetPasswordAsync(ResetPasswordModel criterias);
    }
}
