using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.Resolvers;
using Coco.Entities.Enums;
using Coco.Entities.Dtos.General;
using System;
using System.Threading.Tasks;
using Coco.Entities.Dtos.User;
using System.Collections.Generic;
using Api.Identity.Resolvers.Contracts;
using HotChocolate.Resolvers;
using Coco.Common.Exceptions;
using Coco.Commons.Models;
using Coco.Common.Const;
using Api.Identity.Models;

namespace Api.Identity.Resolvers
{
    public class UserResolver : BaseResolver, IUserResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ILoginManager<ApplicationUser> _loginManager;

        public UserResolver(IUserManager<ApplicationUser> userManager, ILoginManager<ApplicationUser> loginManager)
        {
            _userManager = userManager;
            _loginManager = loginManager;
        }

        public async Task<UpdatePerItemModel> UpdateUserInfoItemAsync(IResolverContext context)
        {
            try
            {
                var model = context.Argument<UpdatePerItemModel>("criterias");
                var userContext = context.ContextData["SessionContext"] as ISessionContext;

                if (!model.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                if (userContext == null || userContext.CurrentUser == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var currentUser = userContext.CurrentUser;

                var result = await _userManager.UpdateInfoItemAsync(model, currentUser.UserIdentityId, currentUser.AuthenticationToken);

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
                if (sessionContext == null || sessionContext.CurrentUser == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var currentUser = sessionContext.CurrentUser;

                var isLoggedout = await _loginManager.LogoutAsync(currentUser.UserIdentityId, currentUser.AuthenticationToken);

                if (!isLoggedout)
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
                var userContext = context.ContextData["SessionContext"] as ISessionContext;

                model.UserPhotoType = UserPhotoTypeEnum.Avatar;
                var result = await _userManager.UpdateAvatarAsync(model, userContext.CurrentUser);

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
                var userContext = context.ContextData["SessionContext"] as ISessionContext;

                model.UserPhotoType = UserPhotoTypeEnum.Cover;
                var result = await _userManager.UpdateCoverAsync(model, userContext.CurrentUser);

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
                var userContext = context.ContextData["SessionContext"] as ISessionContext;

                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                if (userContext == null || userContext.CurrentUser == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var result = await _userManager.DeleteUserPhotoAsync(userContext.CurrentUser.Id, UserPhotoTypeEnum.Avatar);
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
                var userContext = context.ContextData["SessionContext"] as ISessionContext;

                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                if (userContext == null || userContext.CurrentUser == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var result = await _userManager.DeleteUserPhotoAsync(userContext.CurrentUser.Id, UserPhotoTypeEnum.Cover);

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
                var userContext = context.ContextData["SessionContext"] as ISessionContext;

                var currentUser = userContext.CurrentUser;
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
                var model = context.Argument<UserPasswordUpdateDto>("criterias");
                var userContext = context.ContextData["SessionContext"] as ISessionContext;

                var currentUser = userContext.CurrentUser;
                model.UserId = currentUser.Id;

                if (!model.NewPassword.Equals(model.ConfirmPassword))
                {
                    throw new ArgumentException($"{nameof(model.NewPassword)} and {nameof(model.ConfirmPassword)} is not the same");
                }

                var result = await _userManager.ChangePasswordAsync(currentUser.Id, model.CurrentPassword, model.NewPassword);

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
            var userContext = context.ContextData["SessionContext"] as ISessionContext;

            if (!model.CanEdit)
            {
                throw new UnauthorizedAccessException();
            }

            if (userContext == null || userContext.CurrentUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            return model;
        }
        #endregion
    }
}
