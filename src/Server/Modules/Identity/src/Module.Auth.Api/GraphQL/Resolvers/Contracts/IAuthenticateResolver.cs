using Camino.Infrastructure.AspNetCore.Models;
using Module.Auth.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Auth.Api.GraphQL.Resolvers.Contracts
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
