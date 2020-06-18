using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.Models;
using Coco.Framework.Resolvers;
using Coco.Entities.Enums;
using Coco.Entities.Dtos.General;
using System;
using System.Threading.Tasks;
using Api.Auth.Resolvers.Contracts;
using HotChocolate.Resolvers;
using Api.Auth.Models;
using Coco.Business.Contracts;
using Coco.Framework.SessionManager.Core;
using Coco.Framework.Providers.Contracts;
using Coco.Common.Const;
using Coco.Common.Resources;
using MimeKit.Text;
using Coco.Entities.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Coco.Auth.Models;
using AutoMapper;
using Coco.Entities.Dtos.User;
using Coco.Common.Exceptions;
using Coco.Common.Enums;
using Microsoft.Extensions.Options;

namespace Api.Auth.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IUserBusiness _userBusiness;
        private readonly IEmailProvider _emailSender;
        private readonly IMapper _mapper;
        private readonly CocoSettings _cocoSettings;
        private readonly RegisterConfirmationSettings _registerConfirmationSettings;
        private readonly ResetPasswordSettings _resetPasswordSettings;

        public UserResolver(IUserManager<ApplicationUser> userManager, ILoginManager<ApplicationUser> loginManager, IEmailProvider emailSender, 
            IMapper mapper, IUserBusiness userBusiness, SessionState sessionState, IOptions<CocoSettings> cocoSettings, 
            IOptions<RegisterConfirmationSettings> registerConfirmationSettings, IOptions<ResetPasswordSettings> resetPasswordSettings)
            : base(sessionState)
        {
            _userManager = userManager;
            _loginManager = loginManager;
            _mapper = mapper;
            _userBusiness = userBusiness;
            _cocoSettings = cocoSettings.Value;
            _registerConfirmationSettings = registerConfirmationSettings.Value;
            _resetPasswordSettings = resetPasswordSettings.Value;

            _emailSender = emailSender;
        }

        public FullUserInfoModel GetLoggedUser(IResolverContext context)
        {
            return _mapper.Map<FullUserInfoModel>(CurrentUser);
        }

        public async Task<FullUserInfoModel> GetFullUserInfoAsync(IResolverContext context)
        {
            var criterias = context.Argument<FindUserModel>("criterias");
            return await GetFullUserInfoAsync(criterias);
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

        public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<UpdatePerItemModel>("criterias");
                return await UpdateUserInfoItemAsync(criterias);
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync(UpdatePerItemModel criterias)
        {
            ValidateUserInfoItem(criterias);

            var userId = await _userManager.DecryptUserIdAsync(criterias.Key.ToString());
            if (userId != CurrentUser.Id)
            {
                throw new UnauthorizedAccessException();
            }

            var updatePerItem = _mapper.Map<UpdatePerItemDto>(criterias);
            updatePerItem.Key = userId;
            var updatedItem = await _userBusiness.UpdateInfoItemAsync(updatePerItem);
            return _mapper.Map<UpdatePerItemModel>(updatedItem);
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

        public async Task<ICommonResult> SignoutAsync(IResolverContext context)
        {
            try
            {
                var result = await _userManager.RemoveLoginAsync(CurrentUser, ServiceProvidersNameConst.COCO_API_AUTH, CurrentUser.AuthenticationToken);
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
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<UserIdentifierUpdateDto>("criterias");
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
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<UserTokenResult> UpdatePasswordAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<UserPasswordUpdateDto>("criterias");
                ComparePassword(criterias);

                var result = await UpdatePasswordAsync(criterias);
                if (!result.Succeeded)
                {
                    HandleContextError(context, result.Errors);
                    return new UserTokenResult(false);
                }

                return new UserTokenResult()
                {
                    AuthenticationToken = CurrentUser.AuthenticationToken,
                    AccessMode = AccessMode.CanEdit,
                    IsSucceed = true,
                    UserInfo = _mapper.Map<UserInfoModel>(CurrentUser)
                };
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return new UserTokenResult(false);
            }
        }

        public async Task<IdentityResult> UpdatePasswordAsync(UserPasswordUpdateDto criterias)
        {
            var result = await _userManager.ChangePasswordAsync(CurrentUser, criterias.CurrentPassword, criterias.NewPassword);
            return result;
        }

        private void ComparePassword(UserPasswordUpdateDto criterias)
        {
            if (!criterias.NewPassword.Equals(criterias.ConfirmPassword))
            {
                throw new ArgumentException($"{nameof(criterias.NewPassword)} and {nameof(criterias.ConfirmPassword)} is not the same");
            }
        }

        public async Task<UserTokenResult> SigninAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<SigninModel>("criterias");
                var result = await _loginManager.PasswordSignInAsync(model.Username, model.Password, true, true);
                if (!result.Succeeded)
                {
                    throw new UnauthorizedAccessException();
                }

                var user = await _userManager.FindByNameAsync(model.Username);
                var token = await _userManager.GenerateUserTokenAsync(user, ServiceProvidersNameConst.COCO_API_AUTH, Coco.Framework.SessionManager.Core.IdentitySettings.AUTHENTICATION_TOKEN_PURPOSE);
                await _userManager.AddLoginAsync(user, new UserLoginInfo(ServiceProvidersNameConst.COCO_API_AUTH, token, Coco.Framework.SessionManager.Core.IdentitySettings.AUTHENTICATION_TOKEN_PURPOSE));

                var userIdentityId = await _userManager.EncryptUserIdAsync(user.Id);
                return new UserTokenResult(true)
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
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<ICommonResult> SignupAsync(IResolverContext context)
        {
            var model = context.Argument<SignupModel>("criterias");
            var user = new ApplicationUser()
            {
                BirthDate = model.BirthDate,
                DisplayName = $"{model.Lastname} {model.Firstname}",
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                GenderId = (byte)model.GenderId,
                StatusId = (byte)UserStatusEnum.New,
                UserName = model.Email,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(user.UserName);
                    var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await SendActiveEmailAsync(user, confirmationToken);
                }
                else
                {
                    HandleContextError(context, result.Errors);
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
                Body = string.Format(MailTemplateResources.USER_CONFIRMATION_BODY, user.DisplayName, _cocoSettings.ApplicationName, activeUserUrl),
                FromEmail = _registerConfirmationSettings.FromEmail,
                FromName = _registerConfirmationSettings.FromName,
                ToEmail = user.Email,
                ToName = user.DisplayName,
                Subject = string.Format(MailTemplateResources.USER_CONFIRMATION_SUBJECT, _cocoSettings.ApplicationName),
            }, TextFormat.Html);
        }

        public async Task<ICommonResult> ActiveAsync(IResolverContext context)
        {
            var model = context.Argument<ActiveUserModel>("criterias");
            var user = await _userManager.FindByNameAsync(model.Email);
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                var result = await _userManager.ConfirmEmailAsync(user, model.ActiveKey);
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

        public async Task<ICommonResult> ForgotPasswordAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<ForgotPasswordModel>("criterias");
                var result = await ForgotPasswordAsync(criterias);
                if (!result.Succeeded)
                {
                    HandleContextError(context, result.Errors);
                }

                return CommonResult.Success();
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                throw;
            }
        }

        public async Task<IdentityResult> ForgotPasswordAsync(ForgotPasswordModel criterias)
        {
            ValidateForgotPassword(criterias);

            var user = await _userManager.FindByEmailAsync(criterias.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new CocoApplicationException("ForgotPasswordConfirmation");
            }

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.SetAuthenticationTokenAsync(user, ServiceProvidersNameConst.COCO_API_AUTH, Coco.Framework.SessionManager.Core.IdentitySettings.RESET_PASSWORD_PURPOSE, resetPasswordToken);

            if (result.Succeeded)
            {
                await SendPasswordChangeAsync(criterias, user, resetPasswordToken);
            }

            return result;
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
                Body = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_BODY, user.DisplayName, _cocoSettings.ApplicationName, activeUserUrl),
                FromEmail = _resetPasswordSettings.FromEmail,
                FromName = _resetPasswordSettings.FromName,
                ToEmail = user.Email,
                ToName = user.DisplayName,
                Subject = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_SUBJECT, _cocoSettings.ApplicationName),
            }, TextFormat.Html);
        }

        public async Task<ICommonResult> ResetPasswordAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<ResetPasswordModel>("criterias");
                var result = await ResetPasswordAsync(criterias);
                if (!result.Succeeded)
                {
                    HandleContextError(context, result.Errors);
                }
                return CommonResult.Success();
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                throw;
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel criterias)
        {
            ValidateResetPassword(criterias);
            var user = await _userManager.FindByEmailAsync(criterias.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new UnauthorizedAccessException("ResetPasswordFailed");
            }

            var result = await _userManager.ResetPasswordAsync(user, criterias.Key, criterias.Password);
            if (result.Succeeded)
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, ServiceProvidersNameConst.COCO_API_AUTH, criterias.Key);
            }

            return result;
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
