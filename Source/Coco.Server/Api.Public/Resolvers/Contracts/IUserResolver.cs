using Coco.Api.Framework.Models;
using GraphQL.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Public.Resolvers.Contracts
{
    public interface IUserResolver
    {
        ApplicationUser GetLoggedUser(IDictionary<string, object> userContext);
        Task<ApiResult> SigninAsync(ResolveFieldContext<object> context);
        Task<ApiResult> SignupAsync(ResolveFieldContext<object> context);
        Task<ApiResult> GetFullUserInfoAsync(ResolveFieldContext<object> context);
        Task<ApiResult> ForgotPasswordAsync(ResolveFieldContext<object> context);
        Task<ApiResult> ActiveAsync(ResolveFieldContext<object> context);
        Task<ApiResult> ResetPasswordAsync(ResolveFieldContext<object> context);
    }
}
