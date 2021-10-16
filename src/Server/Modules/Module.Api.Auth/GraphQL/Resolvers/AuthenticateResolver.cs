using Camino.Core.Constants;
using Camino.Core.Contracts.Helpers;
using Camino.Core.Contracts.IdentityManager;
using Camino.Core.Contracts.Providers;
using Camino.Core.Domain.Identities;
using Camino.Core.Exceptions;
using Camino.Framework.GraphQL.Resolvers;
using Camino.Framework.Models;
using Camino.IdentityManager.Contracts.Core;
using Camino.Infrastructure.Commons.Constants;
using Camino.Infrastructure.Resources;
using Camino.Shared.Configurations;
using Camino.Shared.Enums;
using Camino.Shared.General;
using Camino.Shared.Requests.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class AuthenticateResolver : BaseResolver, IAuthenticateResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IEmailProvider _emailSender;
        private readonly AppSettings _appSettings;
        private readonly ResetPasswordSettings _resetPasswordSettings;
        private readonly JwtConfigOptions _jwtConfigOptions;
        private readonly IJwtHelper _jwtHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticateResolver(IUserManager<ApplicationUser> userManager, ILoginManager<ApplicationUser> loginManager,
            IEmailProvider emailSender, IOptions<AppSettings> appSettings,
            IOptions<ResetPasswordSettings> resetPasswordSettings, IJwtHelper jwtHelper, IOptions<JwtConfigOptions> jwtConfigOptions,
            IHttpContextAccessor httpContextAccessor)
            : base()
        {
            _userManager = userManager;
            _loginManager = loginManager;
            _appSettings = appSettings.Value;
            _resetPasswordSettings = resetPasswordSettings.Value;
            _emailSender = emailSender;
            _jwtHelper = jwtHelper;
            _jwtConfigOptions = jwtConfigOptions.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserInfoModel> GetLoggedUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            long currentUserId = GetCurrentUserId(claimsPrincipal);
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var userIdentityId = await _userManager.EncryptUserIdAsync(currentUserId);
            return new UserInfoModel
            {
                Address = currentUser.Address,
                BirthDate = currentUser.BirthDate,
                CountryCode = currentUser.CountryCode,
                CountryId = currentUser.CountryId,
                CountryName = currentUser.CountryName,
                Email = currentUser.Email,
                CreatedDate = currentUser.CreatedDate,
                Description = currentUser.Description,
                DisplayName = currentUser.DisplayName,
                Firstname = currentUser.Firstname,
                GenderId = currentUser.GenderId,
                GenderLabel = currentUser.GenderLabel,
                Lastname = currentUser.Lastname,
                PhoneNumber = currentUser.PhoneNumber,
                StatusId = currentUser.StatusId,
                StatusLabel = currentUser.StatusLabel,
                UpdatedDate = currentUser.UpdatedDate,
                UserIdentityId = userIdentityId
            };
        }

        public async Task<UserTokenModel> LoginAsync(LoginModel criterias)
        {
            var result = await _loginManager.PasswordSignInAsync(criterias.Username, criterias.Password, true, true);
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException();
            }

            var user = await _userManager.FindByNameAsync(criterias.Username);
            user.UserIdentityId = await _userManager.EncryptUserIdAsync(user.Id);
            var accessToken = _jwtHelper.GenerateJwtToken(user);

            var refreshToken = await _userManager.GenerateUserTokenAsync(user, ServiceProvidersNameConst.CAMINO_API_AUTH, IdentitySettings.AUTHENTICATION_REFRESH_TOKEN_PURPOSE);
            await _userManager.SetAuthenticationTokenAsync(user, ServiceProvidersNameConst.CAMINO_API_AUTH, IdentitySettings.AUTHENTICATION_REFRESH_TOKEN_PURPOSE, refreshToken);

            AddRefreshTokenToCookie(refreshToken);
            return new UserTokenModel(true)
            {
                AuthenticationToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddHours(_jwtConfigOptions.RefreshTokenHourExpires)
            };
        }

        public async Task<UserTokenModel> RefreshTokenAsync()
        {
            var authenticationToken = GetAccessTokenFromHeader();
            var clientRefreshToken = GetRefreshTokenFromCookie();
            if (string.IsNullOrWhiteSpace(authenticationToken))
            {
                throw new CaminoAuthenticationException();
            }

            if (string.IsNullOrWhiteSpace(clientRefreshToken))
            {
                throw new CaminoAuthenticationException();
            }

            var claimsIdentity = await _jwtHelper.GetPrincipalFromExpiredTokenAsync(authenticationToken);
            var userIdentityId = claimsIdentity.Claims.FirstOrDefault(x => x.Type == HttpHeaderContants.UserIdentityClaimKey).Value;
            if (string.IsNullOrEmpty(userIdentityId))
            {
                return new UserTokenModel();
            }

            var user = await _userManager.FindByIdentityIdAsync(userIdentityId);
            if (user == null)
            {
                return new UserTokenModel();
            }

            var serverRefreshToken = await _userManager.GetUserTokenByValueAsync(user, clientRefreshToken, IdentitySettings.AUTHENTICATION_REFRESH_TOKEN_PURPOSE);
            if (serverRefreshToken == null || serverRefreshToken.ExpiryTime <= DateTimeOffset.Now)
            {
                throw new CaminoAuthenticationException();
            }

            user.UserIdentityId = userIdentityId;
            var accessToken = _jwtHelper.GenerateJwtToken(user);

            await _userManager.RemoveAuthenticationTokenByValueAsync(user.Id, clientRefreshToken);
            var refreshToken = await _userManager.GenerateUserTokenAsync(user, ServiceProvidersNameConst.CAMINO_API_AUTH, IdentitySettings.AUTHENTICATION_REFRESH_TOKEN_PURPOSE);
            await _userManager.SetAuthenticationTokenAsync(user, ServiceProvidersNameConst.CAMINO_API_AUTH, IdentitySettings.AUTHENTICATION_REFRESH_TOKEN_PURPOSE, refreshToken);

            AddRefreshTokenToCookie(refreshToken);
            return new UserTokenModel(true)
            {
                AuthenticationToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddHours(_jwtConfigOptions.RefreshTokenHourExpires)
            };
        }

        private void AddRefreshTokenToCookie(string refreshToken)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(HttpHeaderContants.CookieAuthenticationRefreshToken, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(_jwtConfigOptions.RefreshTokenHourExpires),
                IsEssential = true,
                Secure = true
            });
        }

        private string GetRefreshTokenFromCookie()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[HttpHeaderContants.CookieAuthenticationRefreshToken];
        }

        private string GetAccessTokenFromHeader()
        {
            return _httpContextAccessor.HttpContext.Request.Headers[HttpHeaderContants.HeaderAuthenticationAccessToken];
        }

        public async Task<CommonResult> ForgotPasswordAsync(ForgotPasswordModel criterias)
        {
            ValidateForgotPassword(criterias);

            var user = await _userManager.FindByEmailAsync(criterias.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new CaminoApplicationException("ForgotPasswordConfirmation");
            }

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.SetAuthenticationTokenAsync(user, ServiceProvidersNameConst.CAMINO_API_AUTH, IdentitySettings.RESET_PASSWORD_PURPOSE, resetPasswordToken);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => new CommonError
                {
                    Message = x.Description,
                    Code = x.Code
                });
                return CommonResult.Failed(errors);
            }

            await SendPasswordChangeAsync(criterias, user, resetPasswordToken);

            return CommonResult.Success();
        }

        private void ValidateForgotPassword(ForgotPasswordModel criterias)
        {
            if (criterias == null)
            {
                throw new ArgumentException(nameof(criterias));
            }
        }

        private async Task SendPasswordChangeAsync(ForgotPasswordModel criterias, ApplicationUser user, string token)
        {
            var activeUserUrl = $"{_resetPasswordSettings.Url}/{criterias.Email}/{token}";
            await _emailSender.SendEmailAsync(new MailMessageRequest()
            {
                Body = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_BODY, user.DisplayName, _appSettings.ApplicationName, activeUserUrl),
                FromEmail = _resetPasswordSettings.FromEmail,
                FromName = _resetPasswordSettings.FromName,
                ToEmail = user.Email,
                ToName = user.DisplayName,
                Subject = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_SUBJECT, _appSettings.ApplicationName),
            }, EmailTextFormat.Html);
        }

        public async Task<UserTokenModel> UpdatePasswordAsync(ClaimsPrincipal claimsPrincipal, UserPasswordUpdateModel criterias)
        {
            try
            {
                ComparePassword(criterias);
                var currentUserId = GetCurrentUserId(claimsPrincipal);
                var currentUser = await _userManager.FindByIdAsync(currentUserId);
                var result = await _userManager.ChangePasswordAsync(currentUser, criterias.CurrentPassword, criterias.NewPassword);
                if (!result.Succeeded)
                {
                    return new UserTokenModel(false);
                }

                return await RefreshTokenAsync();
            }
            catch (Exception)
            {
                return new UserTokenModel(false);
            }
        }

        public async Task<CommonResult> ResetPasswordAsync(ResetPasswordModel criterias)
        {
            ValidateResetPassword(criterias);
            var user = await _userManager.FindByEmailAsync(criterias.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new UnauthorizedAccessException("ResetPasswordFailed");
            }

            var result = await _userManager.ResetPasswordAsync(user, criterias.Key, criterias.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => new CommonError()
                {
                    Message = x.Description,
                    Code = x.Code
                });

                return CommonResult.Failed(errors);
            }
            await _userManager.RemoveAuthenticationTokenAsync(user, ServiceProvidersNameConst.CAMINO_API_AUTH, criterias.Key);

            return CommonResult.Success();
        }

        private void ValidateResetPassword(ResetPasswordModel criterias)
        {
            if (criterias == null)
            {
                throw new ArgumentException(nameof(criterias));
            }

            if (!criterias.Password.Equals(criterias.ConfirmPassword))
            {
                throw new ArgumentException($"{nameof(criterias.Password)} and {nameof(criterias.ConfirmPassword)} is not the same");
            }
        }

        private void ComparePassword(UserPasswordUpdateModel criterias)
        {
            if (!criterias.NewPassword.Equals(criterias.ConfirmPassword))
            {
                throw new ArgumentException($"{nameof(criterias.NewPassword)} and {nameof(criterias.ConfirmPassword)} is not the same");
            }
        }
    }
}
