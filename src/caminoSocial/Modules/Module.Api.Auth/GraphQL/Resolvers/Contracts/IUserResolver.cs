using Module.Api.Auth.Models;
using Camino.Framework.Models;
using System.Threading.Tasks;
using Camino.Service.Projections.Request;

namespace Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface IUserResolver
    {
        FullUserInfoModel GetLoggedUser();
        Task<FullUserInfoModel> GetFullUserInfoAsync(FindUserModel criterias);
        Task<UpdatePerItemModel> UpdateUserInfoItemAsync(UpdatePerItemModel criterias);
        Task<ICommonResult> SignoutAsync();
        Task<UserIdentifierUpdateRequest> UpdateIdentifierAsync(UserIdentifierUpdateRequest criterias);
        Task<UserTokenModel> UpdatePasswordAsync(UserPasswordUpdateRequest criterias);

        Task<UserTokenModel> SigninAsync(SigninModel criterias);
        Task<ICommonResult> SignupAsync(SignupModel criterias);
        Task<ICommonResult> ActiveAsync(ActiveUserModel criterias);
        Task<ICommonResult> ForgotPasswordAsync(ForgotPasswordModel criterias);
        Task<ICommonResult> ResetPasswordAsync(ResetPasswordModel criterias);
    }
}
