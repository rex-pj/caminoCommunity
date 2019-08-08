using Coco.Api.Framework.UserIdentity.Contracts;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.Resolvers;
using Coco.Common.Const;
using Coco.Entities.Enums;
using Coco.Entities.Model.General;
using GraphQL;
using GraphQL.Types;
using System;
using System.Threading.Tasks;
using Coco.Entities.Model.User;

namespace Api.Identity.Resolvers
{
    public class UserResolver : BaseResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;

        public UserResolver(IUserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser GetLoggedUser(ISessionContext sessionContext)
        {
            try
            {
                var currentUser = sessionContext.CurrentUser;
                return currentUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResult> UpdateUserInfoItemAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<UpdatePerItemModel>("criterias");
                var userContext = context.UserContext as ISessionContext;

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
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> UpdateAvatarAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = GenerateUserPhotoModel(context);
                var userContext = context.UserContext as ISessionContext;

                model.UserPhotoType = UserPhotoTypeEnum.Avatar;
                var result = await _userManager.UpdateAvatarAsync(model, userContext.CurrentUser.Id);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResult> UpdateCoverAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = GenerateUserPhotoModel(context);
                var userContext = context.UserContext as ISessionContext;

                model.UserPhotoType = UserPhotoTypeEnum.Cover;
                var result = await _userManager.UpdateCoverAsync(model, userContext.CurrentUser.Id);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> DeleteAvatarAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = GenerateUserPhotoModel(context);
                var userContext = context.UserContext as ISessionContext;

                var result = await _userManager.DeleteUserPhotoAsync(userContext.CurrentUser.Id, UserPhotoTypeEnum.Avatar);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> DeleteCoverAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = GenerateUserPhotoModel(context);
                var userContext = context.UserContext as ISessionContext;

                var result = await _userManager.DeleteUserPhotoAsync(userContext.CurrentUser.Id, UserPhotoTypeEnum.Cover);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> UpdateUserProfileAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var user = context.GetArgument<ApplicationUser>("user");
                var userContext = context.UserContext as ISessionContext;

                var currentUser = userContext.CurrentUser;
                user.Id = currentUser.Id;

                var result = await _userManager.UpdateUserProfileAsync(user, currentUser.UserIdentityId, currentUser.AuthenticationToken);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ex.Message, ex);
            }
        }

        public async Task<ApiResult> UpdatePasswordAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<UserPasswordUpdateModel>("criterias");
                var userContext = context.UserContext as ISessionContext;

                var currentUser = userContext.CurrentUser;
                model.UserId = currentUser.Id;

                if (!model.NewPassword.Equals(model.ConfirmPassword))
                {
                    throw new ArgumentException($"{nameof(model.NewPassword)} and {nameof(model.ConfirmPassword)} is not the same");
                }

                var result = await _userManager.ChangePasswordAsync(currentUser.Id, model.CurrentPassword, model.NewPassword);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ex.Message, ex);
            }
        }


        #region Privates
        private UpdateUserPhotoModel GenerateUserPhotoModel(ResolveFieldContext<object> context)
        {
            var model = context.GetArgument<UpdateUserPhotoModel>("criterias");
            var userContext = context.UserContext as ISessionContext;

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
