using Camino.Framework.Models;
using Module.Api.Auth.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface IAuthenticateResolver
    {
        Task<UserTokenModel> RefreshTokenAsync();
        Task<CommonResult> ForgotPasswordAsync(ForgotPasswordModel criterias);
        Task<CommonResult> ResetPasswordAsync(ResetPasswordModel criterias);
        Task<UserTokenModel> UpdatePasswordAsync(ClaimsPrincipal claimsPrincipal, UserPasswordUpdateModel criterias);
        Task<UserInfoModel> GetLoggedUserAsync(ClaimsPrincipal claimsPrincipal);
    }
}
