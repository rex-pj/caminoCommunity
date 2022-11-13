using Camino.Core.DependencyInjection;
using Camino.Infrastructure.Identity.Options;
using Camino.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;

namespace Module.Auth.Api.ModelServices
{
    public class AuthenticationModelService : IAuthenticationModelService, IScopedDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtConfigOptions _jwtConfigOptions;

        public AuthenticationModelService(IHttpContextAccessor httpContextAccessor, IOptions<JwtConfigOptions> jwtConfigOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtConfigOptions = jwtConfigOptions.Value;
        }

        public void AddRefreshTokenToCookie(string refreshToken)
        {
            var isSecure = true;
            var sameSite = SameSiteMode.Lax;
#if DEBUG
            isSecure = false;
            sameSite = SameSiteMode.None;
#endif
            _httpContextAccessor.HttpContext.Response.Cookies.Append(HttpHeaders.CookieAuthenticationRefreshToken, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = sameSite,
                Expires = DateTime.UtcNow.AddDays(_jwtConfigOptions.RefreshTokenHourExpires),
                IsEssential = true,
                Secure = isSecure
            });
        }
    }
}
