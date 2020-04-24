using Coco.Framework.Models;
using Coco.Entities.Dtos.User;
using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IUserResolver
    {
        ApplicationUser GetLoggedUser(IResolverContext context);
        Task<FullUserInfoModel> GetFullUserInfoAsync(IResolverContext context);
        Task<UpdatePerItemModel> UpdateUserInfoItemAsync(IResolverContext context);
        Task<IApiResult> SignoutAsync(IResolverContext context);
        Task<IApiResult> UpdateAvatarAsync(IResolverContext context);
        Task<IApiResult> UpdateCoverAsync(IResolverContext context);
        Task<IApiResult> DeleteAvatarAsync(IResolverContext context);
        Task<IApiResult> DeleteCoverAsync(IResolverContext context);
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(IResolverContext context);
        Task<UserTokenResult> UpdatePasswordAsync(IResolverContext context);

        Task<UserTokenResult> SigninAsync(IResolverContext context);
        Task<IApiResult> SignupAsync(IResolverContext context);
        Task<IApiResult> ForgotPasswordAsync(IResolverContext context);
        Task<IApiResult> ActiveAsync(IResolverContext context);
        Task<UserTokenResult> ResetPasswordAsync(IResolverContext context);
    }
}
