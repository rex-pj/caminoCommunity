using Camino.Framework.Models;
using Camino.Framework.GraphQL.Resolvers;
using Camino.Data.Enums;
using System;
using System.Threading.Tasks;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Module.Api.Auth.Models;
using Camino.Framework.Providers.Contracts;
using Camino.Core.Constants;
using Camino.Core.Resources;
using MimeKit.Text;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Camino.Service.Data.Identity;
using Camino.Core.Exceptions;
using Camino.Core.Enums;
using Microsoft.Extensions.Options;
using Camino.Core.Models;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Models;
using Camino.IdentityManager.Contracts.Core;
using Camino.Framework.Models.Settings;
using Camino.Service.Business.Users.Contracts;
using Camino.Service.Data.Request;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IUserBusiness _userBusiness;
        private readonly IEmailProvider _emailSender;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly RegisterConfirmationSettings _registerConfirmationSettings;
        private readonly ResetPasswordSettings _resetPasswordSettings;

        public UserResolver(IUserManager<ApplicationUser> userManager, ILoginManager<ApplicationUser> loginManager, IEmailProvider emailSender, 
            IMapper mapper, IUserBusiness userBusiness, SessionState sessionState, IOptions<AppSettings> appSettings, 
            IOptions<RegisterConfirmationSettings> registerConfirmationSettings, IOptions<ResetPasswordSettings> resetPasswordSettings)
            : base(sessionState)
        {
            _userManager = userManager;
            _loginManager = loginManager;
            _mapper = mapper;
            _userBusiness = userBusiness;
            _appSettings = appSettings.Value;
            _registerConfirmationSettings = registerConfirmationSettings.Value;
            _resetPasswordSettings = resetPasswordSettings.Value;

            _emailSender = emailSender;
        }

        public FullUserInfoModel GetLoggedUser()
        {
            return _mapper.Map<FullUserInfoModel>(CurrentUser);
        }

        public async Task<FullUserInfoModel> GetFullUserInfoAsync(FindUserModel criterias)
        {
            var userId = CurrentUser.Id;
            if (!string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            var user = await _userBusiness.FindFullByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            var userInfo = _mapper.Map<FullUserInfoModel>(user);
            userInfo.UserIdentityId = CurrentUser.UserIdentityId;
            userInfo.CanEdit = userId == CurrentUser.Id;

            return userInfo;
        }

        public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync(UpdatePerItemModel criterias)
        {
            try
            {
                ValidateUserInfoItem(criterias);

                var userId = await _userManager.DecryptUserIdAsync(criterias.Key.ToString());
                if (userId != CurrentUser.Id)
                {
                    throw new UnauthorizedAccessException();
                }

                var updatePerItem = _mapper.Map<UpdateItemRequest>(criterias);
                updatePerItem.Key = userId;
                var updatedItem = await _userBusiness.UpdateInfoItemAsync(updatePerItem);
                return _mapper.Map<UpdatePerItemModel>(updatedItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        private void ValidateUserInfoItem(UpdatePerItemModel criterias)
        {
            if (!criterias.CanEdit)
            {
                throw new UnauthorizedAccessException();
            }

            if (criterias.PropertyName == null)
            {
                throw new ArgumentException(nameof(criterias.PropertyName));
            }

            if (criterias.Key == null || string.IsNullOrEmpty(criterias.Key.ToString()))
            {
                throw new ArgumentException(nameof(criterias.Key));
            }
        }

        public async Task<ICommonResult> SignoutAsync()
        {
            try
            {
                var result = await _userManager.RemoveLoginAsync(CurrentUser, ServiceProvidersNameConst.CAMINO_API_AUTH, CurrentUser.AuthenticationToken);
                if (!result.Succeeded)
                {
                    return CommonResult.Failed(new CommonError()
                    {
                        Code = ErrorMessageConst.EXCEPTION,
                        Message = ErrorMessageConst.UN_EXPECTED_EXCEPTION
                    });
                }
                return CommonResult.Success();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(UserIdentifierUpdateDto criterias)
        {
            try
            {
                CurrentUser.Lastname = criterias.Lastname;
                CurrentUser.Firstname = criterias.Firstname;
                CurrentUser.DisplayName = criterias.DisplayName;

                var updatedUser = await _userManager.UpdateAsync(CurrentUser);
                if (updatedUser.Succeeded)
                {
                    return new UserIdentifierUpdateDto()
                    {
                        DisplayName = CurrentUser.DisplayName,
                        Firstname = CurrentUser.Firstname,
                        Id = CurrentUser.Id,
                        Lastname = CurrentUser.Lastname
                    };
                }
                return new UserIdentifierUpdateDto();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Camino.Framework.Models.UserTokenResult> UpdatePasswordAsync(UserPasswordUpdateDto criterias)
        {
            try
            {
                ComparePassword(criterias);

                var result = await _userManager.ChangePasswordAsync(CurrentUser, criterias.CurrentPassword, criterias.NewPassword);
                if (!result.Succeeded)
                {
                    return new Camino.Framework.Models.UserTokenResult(false);
                }

                return new Camino.Framework.Models.UserTokenResult()
                {
                    AuthenticationToken = CurrentUser.AuthenticationToken,
                    AccessMode = AccessMode.CanEdit,
                    IsSucceed = true,
                    UserInfo = _mapper.Map<UserInfoModel>(CurrentUser)
                };
            }
            catch (Exception ex)
            {
                return new Camino.Framework.Models.UserTokenResult(false);
            }
        }

        private void ComparePassword(UserPasswordUpdateDto criterias)
        {
            if (!criterias.NewPassword.Equals(criterias.ConfirmPassword))
            {
                throw new ArgumentException($"{nameof(criterias.NewPassword)} and {nameof(criterias.ConfirmPassword)} is not the same");
            }
        }

        public async Task<Camino.Framework.Models.UserTokenResult> SigninAsync(SigninModel criterias)
        {
            try
            {
                var result = await _loginManager.PasswordSignInAsync(criterias.Username, criterias.Password, true, true);
                if (!result.Succeeded)
                {
                    throw new UnauthorizedAccessException();
                }

                var user = await _userManager.FindByNameAsync(criterias.Username);
                var token = await _userManager.GenerateUserTokenAsync(user, ServiceProvidersNameConst.CAMINO_API_AUTH, IdentitySettings.AUTHENTICATION_TOKEN_PURPOSE);
                await _userManager.AddLoginAsync(user, new UserLoginInfo(ServiceProvidersNameConst.CAMINO_API_AUTH, token, IdentitySettings.AUTHENTICATION_TOKEN_PURPOSE));

                var userIdentityId = await _userManager.EncryptUserIdAsync(user.Id);
                return new Camino.Framework.Models.UserTokenResult(true)
                {
                    AuthenticationToken = token,
                    UserInfo = new UserInfoModel()
                    {
                        UserIdentityId = userIdentityId,
                        DisplayName = user.DisplayName
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICommonResult> SignupAsync(SignupModel criterias)
        {
            var user = new ApplicationUser()
            {
                BirthDate = criterias.BirthDate,
                DisplayName = $"{criterias.Lastname} {criterias.Firstname}",
                Email = criterias.Email,
                Firstname = criterias.Firstname,
                Lastname = criterias.Lastname,
                GenderId = (byte)criterias.GenderId,
                StatusId = (byte)UserStatus.New,
                UserName = criterias.Email,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, criterias.Password);
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(user.UserName);
                    var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await SendActiveEmailAsync(user, confirmationToken);
                }
                else
                {
                    return CommonResult.Failed(result.Errors.Select(x => new CommonError() { 
                        Code = x.Code,
                        Message = x.Description
                    }));
                }

                return CommonResult.Success();
            }
            catch (Exception ex)
            {
                return CommonResult.Failed(new CommonError()
                {
                    Message = ex.Message
                });
            }
        }

        private async Task SendActiveEmailAsync(ApplicationUser user, string confirmationToken)
        {
            var activeUserUrl = $"{_registerConfirmationSettings.Url}/{user.Email}/{confirmationToken}";
            await _emailSender.SendEmailAsync(new MailMessageModel()
            {
                Body = string.Format(MailTemplateResources.USER_CONFIRMATION_BODY, user.DisplayName, _appSettings.ApplicationName, activeUserUrl),
                FromEmail = _registerConfirmationSettings.FromEmail,
                FromName = _registerConfirmationSettings.FromName,
                ToEmail = user.Email,
                ToName = user.DisplayName,
                Subject = string.Format(MailTemplateResources.USER_CONFIRMATION_SUBJECT, _appSettings.ApplicationName),
            }, TextFormat.Html);
        }

        public async Task<ICommonResult> ActiveAsync(ActiveUserModel criterias)
        {
            var user = await _userManager.FindByNameAsync(criterias.Email);
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                var result = await _userManager.ConfirmEmailAsync(user, criterias.ActiveKey);
                if (result.Succeeded)
                {
                    return CommonResult.Success();
                }

                var errors = result.Errors.Select(x => new CommonError()
                {
                    Message = x.Description,
                    Code = x.Code
                });

                return CommonResult.Failed(errors);
            }

            return CommonResult.Failed(new CommonError()
            {
                Message = "The user is already confirmed"
            });
        }

        public async Task<ICommonResult> ForgotPasswordAsync(ForgotPasswordModel criterias)
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
            catch (Exception ex)
            {
                throw ex;
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
            await _emailSender.SendEmailAsync(new MailMessageModel()
            {
                Body = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_BODY, user.DisplayName, _appSettings.ApplicationName, activeUserUrl),
                FromEmail = _resetPasswordSettings.FromEmail,
                FromName = _resetPasswordSettings.FromName,
                ToEmail = user.Email,
                ToName = user.DisplayName,
                Subject = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_SUBJECT, _appSettings.ApplicationName),
            }, TextFormat.Html);
        }

        public async Task<ICommonResult> ResetPasswordAsync(ResetPasswordModel criterias)
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
            catch (Exception ex)
            {
                throw ex;
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
