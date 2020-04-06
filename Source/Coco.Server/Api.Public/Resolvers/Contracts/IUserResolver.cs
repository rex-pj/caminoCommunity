using Coco.Api.Framework.Models;
using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace Api.Public.Resolvers.Contracts
{
    public interface IUserResolver
    {
        ApplicationUser GetLoggedUser(IResolverContext context);
        Task<UserTokenResult> SigninAsync(IResolverContext context);
        Task<IApiResult> SignupAsync(IResolverContext context);
        Task<FullUserInfoModel> GetFullUserInfoAsync(IResolverContext context);
        Task<IApiResult> ForgotPasswordAsync(IResolverContext context);
        Task<IApiResult> ActiveAsync(IResolverContext context);
        Task<UserTokenResult> ResetPasswordAsync(IResolverContext context);
    }
}
