using Api.Auth.Models;
using Coco.Entities.Dtos.User;
using Coco.Framework.Models;
using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IUserResolver
    {
        FullUserInfoModel GetLoggedUser(IResolverContext context);
        Task<FullUserInfoModel> GetFullUserInfoAsync(IResolverContext context);
        Task<UpdatePerItemModel> UpdateUserInfoItemAsync(IResolverContext context);
        Task<ICommonResult> SignoutAsync(IResolverContext context);
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(IResolverContext context);
        Task<UserTokenResult> UpdatePasswordAsync(IResolverContext context);

        Task<UserTokenResult> SigninAsync(IResolverContext context);
        Task<ICommonResult> SignupAsync(IResolverContext context);
        Task<ICommonResult> ActiveAsync(IResolverContext context);
        Task<ICommonResult> ForgotPasswordAsync(IResolverContext context);
        Task<ICommonResult> ResetPasswordAsync(IResolverContext context);
    }
}
