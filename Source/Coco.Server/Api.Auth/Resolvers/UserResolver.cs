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
using Microsoft.AspNetCore.Http;
using Coco.Framework.SessionManager.Core;
using Coco.Framework.Services.Contracts;
using Coco.Common.Const;
using Microsoft.Extensions.Configuration;
using Coco.Common.Resources;
using MimeKit.Text;
using Coco.Entities.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Coco.Auth.Models;
using AutoMapper;

namespace Api.Auth.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IUserPhotoBusiness _userPhotoBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _appName;
        private readonly string _registerConfirmUrl;
        private readonly string _resetPasswordUrl;
        private readonly string _registerConfirmFromEmail;
        private readonly string _registerConfirmFromName;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public UserResolver(IUserManager<ApplicationUser> userManager, ILoginManager<ApplicationUser> loginManager, 
            IUserPhotoBusiness userPhotoBusiness, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, 
            IEmailSender emailSender, IMapper mapper, IUserBusiness userBusiness)
        {
            _userManager = userManager;
            _loginManager = loginManager;
            _userPhotoBusiness = userPhotoBusiness;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userBusiness = userBusiness;

            _emailSender = emailSender;
            _appName = configuration[ConfigurationSettingsConst.APPLICATION_NAME];
            _registerConfirmUrl = configuration[ConfigurationSettingsConst.REGISTER_CONFIRMATION_URL];
            _registerConfirmFromEmail = configuration[ConfigurationSettingsConst.REGISTER_CONFIRM_FROM_EMAIL];
            _registerConfirmFromName = configuration[ConfigurationSettingsConst.REGISTER_CONFIRM_FROM_NAME];
            _resetPasswordUrl = configuration[ConfigurationSettingsConst.RESET_PASSWORD_URL];
        }

        public async Task<ApplicationUser> GetLoggedUserAsync(IResolverContext context)
        {
            try
            {
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                var currentUser = await sessionContext.GetLoggedUserAsync();
                return currentUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FullUserInfoModel> GetFullUserInfoAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<FindUserModel>("criterias");
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                
                long userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
                var currentUser = await sessionContext.GetLoggedUserAsync();
                if (string.IsNullOrEmpty(criterias.UserId))
                {
                    userId = currentUser.Id;
                }
                
                var user = await _userBusiness.FindFullByIdAsync(userId);

                var userInfo = _mapper.Map<FullUserInfoModel>(user);
                userInfo.UserIdentityId = currentUser.UserIdentityId;
                userInfo.CanEdit = userId == currentUser.Id;

                return userInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<UpdatePerItemModel>("criterias");
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                if (criterias.PropertyName == null)
                {
                    throw new ArgumentNullException(nameof(criterias.PropertyName));
                }

                if (criterias.Key == null || string.IsNullOrEmpty(criterias.Key.ToString()))
                {
                    throw new ArgumentNullException(nameof(criterias.Key));
                }

                var userContext = context.ContextData["SessionContext"] as ISessionContext;
                var currentUser = await userContext.GetLoggedUserAsync();

                var userId = await _userManager.DecryptUserIdAsync(criterias.Key.ToString());
                if(userId != currentUser.Id)
                {
                    throw new UnauthorizedAccessException();
                }

                var updatePerItem = _mapper.Map<UpdatePerItemDto>(criterias);
                updatePerItem.Key = userId;
                var updatedItem = await _userBusiness.UpdateInfoItemAsync(updatePerItem);
                return _mapper.Map<UpdatePerItemModel>(updatedItem);
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<ICommonResult> SignoutAsync(IResolverContext context)
        {
            try
            {
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                var currentUser = await sessionContext.GetLoggedUserAsync();

                var result = await _userManager.RemoveLoginAsync(currentUser, ServiceProvidersNameConst.COCO_API_AUTH, currentUser.AuthenticationToken);
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

        //public async Task<ICommonResult> UpdateAvatarAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var model = GenerateUserPhotoModel(context);
        //        var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
        //        var currentUser = sessionContext.CurrentUser;

        //        var user = await _userManagerOld.FindUserByIdentityIdAsync(currentUser.UserIdentityId, currentUser.AuthenticationToken);
        //        if (user == null)
        //        {
        //            throw new UnauthorizedAccessException();
        //        }

        //        model.UserPhotoType = UserPhotoTypeEnum.Avatar;
        //        var result = await _userPhotoBusiness.UpdateUserPhotoAsync(model, user.Id);

        //        return CommonResult.Success(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleContextError(context, ex);
        //        return null;
        //    }
        //}

        //public async Task<ICommonResult> UpdateCoverAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var model = GenerateUserPhotoModel(context);
        //        var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
        //        var currentUser = sessionContext.CurrentUser;

        //        var user = await _userManagerOld.FindUserByIdentityIdAsync(currentUser.UserIdentityId, currentUser.AuthenticationToken);
        //        if (user == null)
        //        {
        //            throw new UnauthorizedAccessException();
        //        }

        //        model.UserPhotoType = UserPhotoTypeEnum.Cover;
        //        var result = await _userPhotoBusiness.UpdateUserPhotoAsync(model, user.Id);

        //        return CommonResult.Success(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleContextError(context, ex);
        //        return null;
        //    }
        //}

        //public async Task<ICommonResult> DeleteAvatarAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var criterias = context.Argument<PhotoDeleteModel>("criterias");

        //        if (!criterias.CanEdit)
        //        {
        //            throw new UnauthorizedAccessException();
        //        }

        //        var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
        //        var currentUser = sessionContext.CurrentUser;
        //        if (currentUser.Id < 0)
        //        {
        //            throw new ArgumentNullException(nameof(currentUser.Id));
        //        }

        //        await _userPhotoBusiness.DeleteUserPhotoAsync(currentUser.Id, UserPhotoTypeEnum.Avatar);
        //        return CommonResult.Success(new UserPhotoUpdateDto());
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleContextError(context, ex);
        //        return null;
        //    }
        //}

        //public async Task<ICommonResult> DeleteCoverAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var criterias = context.Argument<PhotoDeleteModel>("criterias");

        //        if (!criterias.CanEdit)
        //        {
        //            throw new UnauthorizedAccessException();
        //        }

        //        var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
        //        await _userPhotoBusiness.DeleteUserPhotoAsync(sessionContext.CurrentUser.Id, UserPhotoTypeEnum.Cover);
        //        return CommonResult.Success(new UserPhotoUpdateDto());
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleContextError(context, ex);
        //        return null;
        //    }
        //}

        //public async Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var criterias = context.Argument<UserIdentifierUpdateDto>("criterias");
        //        var sessionContext = context.ContextData["SessionContext"] as ISessionContext;

        //        var currentUser = sessionContext.CurrentUser;
        //        currentUser.Lastname = criterias.Lastname;
        //        currentUser.Firstname = criterias.Firstname;
        //        currentUser.DisplayName = criterias.DisplayName;

        //        return await _userManagerOld.UpdateIdentifierAsync(currentUser, currentUser.UserIdentityId, currentUser.AuthenticationToken);
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleContextError(context, ex);
        //        return null;
        //    }
        //}

        //public async Task<UserTokenResult> UpdatePasswordAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var criterias = context.Argument<UserPasswordUpdateDto>("criterias");
        //        var sessionContext = context.ContextData["SessionContext"] as ISessionContext;

        //        var currentUser = sessionContext.CurrentUser;
        //        criterias.UserId = currentUser.Id;

        //        if (!criterias.NewPassword.Equals(criterias.ConfirmPassword))
        //        {
        //            throw new ArgumentException($"{nameof(criterias.NewPassword)} and {nameof(criterias.ConfirmPassword)} is not the same");
        //        }

        //        var result = await _userManagerOld.ChangePasswordAsync(currentUser.Id, criterias.CurrentPassword, criterias.NewPassword);

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleContextError(context, ex);
        //        return new UserTokenResult(false);
        //    }
        //}

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
                var token = _userManager.GenerateNewAuthenticatorKey();
                await _userManager.AddLoginAsync(user, new UserLoginInfo(ServiceProvidersNameConst.COCO_API_AUTH, token, UserAttributeOptions.AUTHENTICATION_TOKEN));

                var userIdentityId = await _userManager.EncryptUserIdAsync(user.Id);
                return new UserTokenResult(true) {
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
            try
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

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(user.UserName);
                    var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await SendActiveEmailAsync(user, confirmationToken);
                }

                return CommonResult.Success();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task SendActiveEmailAsync(ApplicationUser user, string confirmationToken)
        {
            try
            {
                var activeUserUrl = $"{_registerConfirmUrl}/{user.Email}/{confirmationToken}";
                await _emailSender.SendEmailAsync(new MailMessageModel()
                {
                    Body = string.Format(MailTemplateResources.USER_CONFIRMATION_BODY, user.DisplayName, _appName, activeUserUrl),
                    FromEmail = _registerConfirmFromEmail,
                    FromName = _registerConfirmFromName,
                    ToEmail = user.Email,
                    ToName = user.DisplayName,
                    Subject = string.Format(MailTemplateResources.USER_CONFIRMATION_SUBJECT, _appName),
                }, TextFormat.Html);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICommonResult> ActiveAsync(IResolverContext context)
        {
            try
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

                    var errors = result.Errors.Select(x => new CommonError() { 
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<ICommonResult> ForgotPasswordAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var criterias = context.Argument<ForgotPasswordModel>("criterias");
        //        if (criterias == null)
        //        {
        //            throw new NullReferenceException(nameof(criterias));
        //        }

        //        var user = await _userManagerOld.FindByEmailAsync(criterias.Email);
        //        if (user == null || !(await _userManagerOld.IsEmailConfirmedAsync(user)))
        //        {
        //            throw new ApplicationException("ForgotPasswordConfirmation");
        //        }

        //        var result = await _userManagerOld.ForgotPasswordAsync(criterias.Email);

        //        var response = result as CommonResult;
        //        var userResponse = response.Result.ToString();
        //        var activeUserUrl = $"{_resetPasswordUrl}/{criterias.Email}/{userResponse}";
        //        await _emailSender.SendEmailAsync(new MailMessageModel()
        //        {
        //            Body = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_BODY, user.DisplayName, _appName, activeUserUrl),
        //            FromEmail = _registerConfirmFromEmail,
        //            FromName = _registerConfirmFromName,
        //            ToEmail = user.Email,
        //            ToName = user.DisplayName,
        //            Subject = string.Format(MailTemplateResources.USER_CHANGE_PASWORD_CONFIRMATION_SUBJECT, _appName),
        //        }, TextFormat.Html);

        //        if (!result.IsSucceed)
        //        {
        //            HandleContextError(context, result.Errors);
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<UserTokenResult> ResetPasswordAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var model = context.Argument<ResetPasswordModel>("criterias");

        //        if (model == null)
        //        {
        //            throw new NullReferenceException(nameof(model));
        //        }

        //        if (!model.Password.Equals(model.ConfirmPassword))
        //        {
        //            throw new ArgumentException($"{nameof(model.Password)} and {nameof(model.ConfirmPassword)} is not the same");
        //        }

        //        var user = await _userManagerOld.FindByEmailAsync(model.Email);
        //        if (user == null || !(await _userManagerOld.IsEmailConfirmedAsync(user)))
        //        {
        //            throw new UnauthorizedAccessException("ResetPasswordFailed");
        //        }

        //        var result = await _userManagerOld.ResetPasswordAsync(model, user.Id);

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #region Privates
        private UserPhotoUpdateDto GenerateUserPhotoModel(IResolverContext context)
        {
            var model = context.Argument<UserPhotoUpdateDto>("criterias");
            if (!model.CanEdit)
            {
                throw new UnauthorizedAccessException();
            }

            return model;
        }
        #endregion
    }
}
