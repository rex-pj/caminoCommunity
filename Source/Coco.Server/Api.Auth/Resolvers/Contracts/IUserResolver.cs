using Coco.Framework.Models;
using Coco.Entities.Dtos.User;
using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IUserResolver
    {
        //ApplicationUser GetLoggedUser(IResolverContext context);
        //Task<FullUserInfoModel> GetFullUserInfoAsync(IResolverContext context);
        //Task<UpdatePerItemModel> UpdateUserInfoItemAsync(IResolverContext context);
        //Task<ICommonResult> SignoutAsync(IResolverContext context);
        //Task<ICommonResult> UpdateAvatarAsync(IResolverContext context);
        //Task<ICommonResult> UpdateCoverAsync(IResolverContext context);
        //Task<ICommonResult> DeleteAvatarAsync(IResolverContext context);
        //Task<ICommonResult> DeleteCoverAsync(IResolverContext context);
        //Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(IResolverContext context);
        //Task<UserTokenResult> UpdatePasswordAsync(IResolverContext context);

        //Task<UserTokenResult> SigninAsync(IResolverContext context);
        Task<ICommonResult> SignupAsync(IResolverContext context);
        //Task<ICommonResult> ForgotPasswordAsync(IResolverContext context);
        //Task<ICommonResult> ActiveAsync(IResolverContext context);
        //Task<UserTokenResult> ResetPasswordAsync(IResolverContext context);
    }
}
