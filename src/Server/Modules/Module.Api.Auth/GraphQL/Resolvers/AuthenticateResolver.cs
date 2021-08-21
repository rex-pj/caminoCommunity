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
using Microsoft.Extensions.Options;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using System;
using System.Linq;
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
        private readonly IJwtHelper _jwtHelper;

        public AuthenticateResolver(IUserManager<ApplicationUser> userManager, ILoginManager<ApplicationUser> loginManager,
            IEmailProvider emailSender, IOptions<AppSettings> appSettings,
            IOptions<ResetPasswordSettings> resetPasswordSettings, IJwtHelper jwtHelper)
            : base()
        {
            _userManager = userManager;
            _loginManager = loginManager;
            _appSettings = appSettings.Value;
            _resetPasswordSettings = resetPasswordSettings.Value;
            _emailSender = emailSender;
            _jwtHelper = jwtHelper;
        }

        public async Task<UserTokenModel> LoginAsync(LoginModel criterias)
        {
            try
            {
                var result = await _loginManager.PasswordSignInAsync(criterias.Username, criterias.Password, true, true);
                if (!result.Succeeded)
                {
                    throw new UnauthorizedAccessException();
                }

                var user = await _userManager.FindByNameAsync(criterias.Username);
                user.UserIdentityId = await _userManager.EncryptUserIdAsync(user.Id);
                var jwtToken = _jwtHelper.GenerateJwtToken(user);

                var userIdentityId = await _userManager.EncryptUserIdAsync(user.Id);
                return new UserTokenModel(true)
                {
                    AuthenticationToken = jwtToken,
                    UserInfo = new UserInfoModel()
                    {
                        UserIdentityId = userIdentityId,
                        DisplayName = user.DisplayName
                    }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommonResult> ForgotPasswordAsync(ForgotPasswordModel criterias)
        {
            try
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
                    var errors = result.Errors.Select(x => new CommonError()
                    {
                        Message = x.Description,
                        Code = x.Code
                    });
                    return CommonResult.Failed(errors);
                }

                await SendPasswordChangeAsync(criterias, user, resetPasswordToken);

                return CommonResult.Success();
            }
            catch (Exception)
            {
                throw;
            }
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

        public async Task<CommonResult> ResetPasswordAsync(ResetPasswordModel criterias)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
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
    }
}
