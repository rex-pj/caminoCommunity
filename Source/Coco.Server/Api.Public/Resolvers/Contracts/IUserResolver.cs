using Coco.Api.Framework.Models;
using HotChocolate.Resolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Public.Resolvers.Contracts
{
    public interface IUserResolver
    {
        ApplicationUser GetLoggedUser(IDictionary<string, object> userContext);
        Task<ApiResult> SigninAsync(IResolverContext context);
        Task<ApiResult> SignupAsync(IResolverContext context);
        Task<ApiResult> GetFullUserInfoAsync(IResolverContext context);
        Task<ApiResult> ForgotPasswordAsync(IResolverContext context);
        Task<ApiResult> ActiveAsync(IResolverContext context);
        Task<ApiResult> ResetPasswordAsync(IResolverContext context);
    }
}
