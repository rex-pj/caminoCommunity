using Camino.Core.DependencyInjection;
using Camino.Infrastructure.Emails.Contracts.Dtos;
using Camino.Infrastructure.Emails.Contracts;
using Camino.Infrastructure.Emails.Templates;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Options;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Constants;
using Camino.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Module.Auth.Api.Models;
using System;
using System.Threading.Tasks;

namespace Module.Auth.Api.ModelServices
{
    public class AuthenticationModelService : IAuthenticationModelService, IScopedDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtConfigOptions _jwtConfigOptions;
        private readonly IEmailClient _emailClient;
        private readonly ResetPasswordSettings _resetPasswordSettings;
        private readonly ApplicationSettings _appSettings;

        public AuthenticationModelService(IHttpContextAccessor httpContextAccessor, 
            IOptions<JwtConfigOptions> jwtConfigOptions,
            IEmailClient emailClient,
            IOptions<ApplicationSettings> appSettings,
            IOptions<ResetPasswordSettings> resetPasswordSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtConfigOptions = jwtConfigOptions.Value;
            _emailClient = emailClient;
            _appSettings = appSettings?.Value;
            _resetPasswordSettings = resetPasswordSettings?.Value;
        }

        public void AddRefreshTokenToCookie(string refreshToken)
        {
            var isSecure = false;
            var sameSite = SameSiteMode.Lax;
 #if !DEBUG
            isSecure = true;
            sameSite = SameSiteMode.Lax;
#endif
            _httpContextAccessor.HttpContext.Response.Cookies.Append(HttpHeaders.CookieAuthenticationRefreshToken, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = sameSite,
                Expires = DateTime.UtcNow.AddHours(_jwtConfigOptions.RefreshTokenHourExpires),
                IsEssential = true,
                Secure = isSecure
            });
        }

        public string GetRefreshTokenFromCookie()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[HttpHeaders.CookieAuthenticationRefreshToken];
        }

        public string GetAccessTokenFromHeader()
        {
            return _httpContextAccessor.HttpContext.Request.Headers[HttpHeaders.HeaderAuthenticationAccessToken];
        }

        public async Task SendPasswordChangeAsync(ForgotPasswordModel criterias, ApplicationUser user, string token)
        {
            var activeUserUrl = $"{_resetPasswordSettings.Url}/{criterias.Email}/{token}";
            await _emailClient.SendEmailAsync(new MailMessageRequest()
            {
                Body = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_BODY, user.DisplayName, _appSettings.ApplicationName, activeUserUrl),
                FromEmail = _resetPasswordSettings.FromEmail,
                FromName = _resetPasswordSettings.FromName,
                ToEmail = user.Email,
                ToName = user.DisplayName,
                Subject = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_SUBJECT, _appSettings.ApplicationName),
            }, EmailTextFormats.Html);
        }
    }
}
