using Camino.Infrastructure.Identity.Core;
using Module.Auth.Api.Models;
using System.Threading.Tasks;

namespace Module.Auth.Api.ModelServices
{
    public interface IAuthenticationModelService
    {
        void AddRefreshTokenToCookie(string refreshToken);
        string GetRefreshTokenFromCookie();
        string GetAccessTokenFromHeader();
        Task SendPasswordChangeAsync(ForgotPasswordModel criterias, ApplicationUser user, string token);
    }
}