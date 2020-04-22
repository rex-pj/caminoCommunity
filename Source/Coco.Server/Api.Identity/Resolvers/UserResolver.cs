using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.Models;
using Coco.Framework.Resolvers;
using Coco.Entities.Enums;
using Coco.Entities.Dtos.General;
using System;
using System.Threading.Tasks;
using Coco.Entities.Dtos.User;
using Api.Identity.Resolvers.Contracts;
using HotChocolate.Resolvers;
using Coco.Commons.Models;
using Coco.Common.Const;
using Api.Identity.Models;
using AutoMapper;

namespace Api.Identity.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IMapper _mapper;

        public UserResolver(IUserManager<ApplicationUser> userManager, ILoginManager<ApplicationUser> loginManager, IMapper mapper)
        {
            _userManager = userManager;
            _loginManager = loginManager;
            _mapper = mapper;
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

        public async Task<FullUserInfoModel> GetFullUserInfoAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<FindUserModel>("criterias");
                var userIdentityId = criterias.UserId;

                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                if (string.IsNullOrEmpty(criterias.UserId))
                {
                    userIdentityId = sessionContext.CurrentUser.UserIdentityId;
                }

                var user = await _userManager.FindUserByIdentityIdAsync(userIdentityId, sessionContext.AuthenticationToken);

                var userInfo = _mapper.Map<FullUserInfoModel>(user);
                userInfo.UserIdentityId = userIdentityId;
                userInfo.CanEdit = sessionContext.CurrentUser != null
                    && !string.IsNullOrEmpty(user.AuthenticationToken)
                    && user.AuthenticationToken.Equals(sessionContext.AuthenticationToken);

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
                var userContext = context.ContextData["SessionContext"] as ISessionContext;

                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var currentUser = userContext.CurrentUser;

                var result = await _userManager.UpdateInfoItemAsync(criterias, currentUser.UserIdentityId, currentUser.AuthenticationToken);

                return result;
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<IApiResult> SignoutAsync(IResolverContext context)
        {
            try
            {
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                var currentUser = sessionContext.CurrentUser;

                var isLoggedOut = await _loginManager.LogoutAsync(currentUser.UserIdentityId, currentUser.AuthenticationToken);
                if (!isLoggedOut)
                {
                    return ApiResult.Failed(new CommonError()
                    {
                        Code = ErrorMessageConst.EXCEPTION,
                        Message = ErrorMessageConst.UN_EXPECTED_EXCEPTION
                    });
                }
                return ApiResult.Success();
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<IApiResult> UpdateAvatarAsync(IResolverContext context)
        {
            try
            {
                var model = GenerateUserPhotoModel(context);
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;

                model.UserPhotoType = UserPhotoTypeEnum.Avatar;
                var result = await _userManager.UpdateAvatarAsync(model, sessionContext.CurrentUser);

                return ApiResult.Success(result);
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<IApiResult> UpdateCoverAsync(IResolverContext context)
        {
            try
            {
                var model = GenerateUserPhotoModel(context);
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;

                model.UserPhotoType = UserPhotoTypeEnum.Cover;
                var result = await _userManager.UpdateCoverAsync(model, sessionContext.CurrentUser);

                return ApiResult.Success(result);
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<IApiResult> DeleteAvatarAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<PhotoDeleteModel>("criterias");

                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                var result = await _userManager.DeleteUserPhotoAsync(sessionContext.CurrentUser.Id, UserPhotoTypeEnum.Avatar);
                return ApiResult.Success(result);
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<IApiResult> DeleteCoverAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<PhotoDeleteModel>("criterias");

                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                var result = await _userManager.DeleteUserPhotoAsync(sessionContext.CurrentUser.Id, UserPhotoTypeEnum.Cover);
                return ApiResult.Success(result);
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
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;

                var currentUser = sessionContext.CurrentUser;
                currentUser.Lastname = criterias.Lastname;
                currentUser.Firstname = criterias.Firstname;
                currentUser.DisplayName = criterias.DisplayName;

                return await _userManager.UpdateIdentifierAsync(currentUser, currentUser.UserIdentityId, currentUser.AuthenticationToken);
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
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;

                var currentUser = sessionContext.CurrentUser;
                criterias.UserId = currentUser.Id;

                if (!criterias.NewPassword.Equals(criterias.ConfirmPassword))
                {
                    throw new ArgumentException($"{nameof(criterias.NewPassword)} and {nameof(criterias.ConfirmPassword)} is not the same");
                }

                var result = await _userManager.ChangePasswordAsync(currentUser.Id, criterias.CurrentPassword, criterias.NewPassword);

                return result;
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return new UserTokenResult(false);
            }
        }

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
