using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.Mapping;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.Resolvers;
using Coco.Common.Const;
using Coco.Entities.Enums;
using Coco.Entities.Model.General;
using GraphQL;
using GraphQL.Types;
using System;
using System.Threading.Tasks;

namespace Api.Identity.Resolvers
{
    public class AccountResolver : BaseResolver
    {
        private readonly IAccountManager<ApplicationUser> _accountManager;

        public AccountResolver(IAccountManager<ApplicationUser> accountManager)
        {
            _accountManager = accountManager;
        }

        public UserInfo GetLoggedUser(IWorkContext workContext)
        {
            try
            {
                var currentUser = workContext.CurrentUser;
                return UserInfoMapping.ApplicationUserToUserInfo(currentUser,
                    workContext.CurrentUser.UserIdentityId);
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
                var userContext = context.UserContext as IWorkContext;

                if (!model.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                if (userContext == null || userContext.CurrentUser == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var result = await _accountManager.UpdateInfoItemAsync(model, userContext.CurrentUser.AuthenticationToken);
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
                var userContext = context.UserContext as IWorkContext;

                model.UserPhotoType = UserPhotoTypeEnum.Avatar;
                var result = await _accountManager.UpdateAvatarAsync(model, userContext.CurrentUser.Id);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> UpdateCoverAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = GenerateUserPhotoModel(context);
                var userContext = context.UserContext as IWorkContext;

                model.UserPhotoType = UserPhotoTypeEnum.Cover;
                var result = await _accountManager.UpdateCoverAsync(model, userContext.CurrentUser.Id);
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
                var userContext = context.UserContext as IWorkContext;

                var result = await _accountManager.DeleteUserPhotoAsync(userContext.CurrentUser.Id, UserPhotoTypeEnum.Avatar);
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
                var userContext = context.UserContext as IWorkContext;

                var result = await _accountManager.DeleteUserPhotoAsync(userContext.CurrentUser.Id, UserPhotoTypeEnum.Cover);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        #region Privates
        private UpdateUserPhotoModel GenerateUserPhotoModel(ResolveFieldContext<object> context)
        {
            var model = context.GetArgument<UpdateUserPhotoModel>("criterias");
            var userContext = context.UserContext as IWorkContext;

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
