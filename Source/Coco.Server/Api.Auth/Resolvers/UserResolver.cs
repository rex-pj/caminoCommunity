using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.Models;
using Coco.Framework.Resolvers;
using Coco.Entities.Enums;
using Coco.Entities.Dtos.General;
using System;
using System.Threading.Tasks;
using Coco.Entities.Dtos.User;
using Api.Auth.Resolvers.Contracts;
using HotChocolate.Resolvers;
using Coco.Entities.Models;
using Coco.Common.Const;
using AutoMapper;
using Coco.Framework.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Api.Auth.Models;
using Coco.Auth.Models;
using Coco.Common.Resources;
using MimeKit.Text;
using Coco.Business.Contracts;
using Coco.Framework.SessionManager;

namespace Api.Auth.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        //private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly string _appName;
        private readonly string _registerConfirmUrl;
        private readonly string _resetPasswordUrl;
        private readonly string _registerConfirmFromEmail;
        private readonly string _registerConfirmFromName;
        private readonly IUserPhotoBusiness _userPhotoBusiness;

        public UserResolver(IUserManager<ApplicationUser> userManager, 
            //ILoginManager<ApplicationUser> loginManager, 
            IMapper mapper, IEmailSender emailSender, IUserPhotoBusiness userPhotoBusiness,
            IConfiguration configuration)
        {
            _userManager = userManager;
            //_loginManager = loginManager;
            _mapper = mapper;
            _userPhotoBusiness = userPhotoBusiness;

            _emailSender = emailSender;
            _appName = configuration["ApplicationName"];
            _registerConfirmUrl = configuration["RegisterConfimation:Url"];
            _registerConfirmFromEmail = configuration["RegisterConfimation:FromEmail"];
            _registerConfirmFromName = configuration["RegisterConfimation:FromName"];
            _resetPasswordUrl = configuration["ResetPassword:Url"];
        }

        public ApplicationUser GetLoggedUser(IResolverContext context)
        {
            try
            {
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                var currentUser = sessionContext.CurrentUser;
                return currentUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<FullUserInfoModel> GetFullUserInfoAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var criterias = context.Argument<FindUserModel>("criterias");
        //        var userIdentityId = criterias.UserId;

        //        var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
        //        if (string.IsNullOrEmpty(criterias.UserId))
        //        {
        //            userIdentityId = sessionContext.CurrentUser.UserIdentityId;
        //        }

        //        var user = await _userManagerOld.FindUserByIdentityIdAsync(userIdentityId, sessionContext.AuthenticationToken);

        //        var userInfo = _mapper.Map<FullUserInfoModel>(user);
        //        userInfo.UserIdentityId = userIdentityId;
        //        userInfo.CanEdit = sessionContext.CurrentUser != null
        //            && !string.IsNullOrEmpty(user.AuthenticationToken)
        //            && user.AuthenticationToken.Equals(sessionContext.AuthenticationToken);

        //        return userInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        //public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var criterias = context.Argument<UpdatePerItemModel>("criterias");
        //        var userContext = context.ContextData["SessionContext"] as ISessionContext;

        //        if (!criterias.CanEdit)
        //        {
        //            throw new UnauthorizedAccessException();
        //        }

        //        var currentUser = userContext.CurrentUser;

        //        var result = await _userManagerOld.UpdateInfoItemAsync(criterias, currentUser.UserIdentityId, currentUser.AuthenticationToken);

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleContextError(context, ex);
        //        return null;
        //    }
        //}

        //public async Task<ICommonResult> SignoutAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
        //        var currentUser = sessionContext.CurrentUser;

        //        var isLoggedOut = await _loginManager.LogoutAsync(currentUser);
        //        if (!isLoggedOut)
        //        {
        //            return CommonResult.Failed(new CommonError()
        //            {
        //                Code = ErrorMessageConst.EXCEPTION,
        //                Message = ErrorMessageConst.UN_EXPECTED_EXCEPTION
        //            });
        //        }
        //        return CommonResult.Success();
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleContextError(context, ex);
        //        return null;
        //    }
        //}

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

        //public async Task<UserTokenResult> SigninAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var model = context.Argument<SigninModel>("criterias");
        //        var result = await _loginManager.LoginAsync(model.Username, model.Password);
        //        if (!result.IsSucceed)
        //        {
        //            HandleContextError(context, result.Errors);
        //        }

        //        return result.Result as UserTokenResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleContextError(context, ex);
        //        return null;
        //    }
        //}

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
                    Password = model.Password,
                };

                var result = await _userManager.CreateAsync(user);
                //if (result.Succeeded)
                //{
                //    var response = result as CommonResult;
                //    var userResponse = response.Result as ApplicationUser;
                //    var activeUserUrl = $"{_registerConfirmUrl}/{user.Email}/{userResponse.ActiveUserStamp}";
                //    await _emailSender.SendEmailAsync(new MailMessageModel()
                //    {
                //        Body = string.Format(MailTemplateResources.USER_CONFIRMATION_BODY, user.DisplayName, _appName, activeUserUrl),
                //        FromEmail = _registerConfirmFromEmail,
                //        FromName = _registerConfirmFromName,
                //        ToEmail = user.Email,
                //        ToName = user.DisplayName,
                //        Subject = string.Format(MailTemplateResources.USER_CONFIRMATION_SUBJECT, _appName),
                //    }, TextFormat.Html);
                //}
                //else
                //{
                //    HandleContextError(context, result.Errors);
                //}

                return CommonResult.Success();
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

        //public async Task<ICommonResult> ActiveAsync(IResolverContext context)
        //{
        //    try
        //    {
        //        var model = context.Argument<ActiveUserModel>("criterias");

        //        var result = await _userManagerOld.ActiveAsync(model.Email, model.ActiveKey);

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
