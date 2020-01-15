using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.Resolvers;
using Coco.Common.Const;
using Coco.Entities.Enums;
using Coco.Entities.Dtos.General;
using GraphQL;
using GraphQL.Types;
using System;
using System.Threading.Tasks;
using Coco.Entities.Dtos.User;
using System.Collections.Generic;
using Api.Identity.Resolvers.Contracts;

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

        public async Task<ApiResult> UpdateUserInfoItemAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<UpdatePerItemModel>("criterias");
                var userContext = context.UserContext["SessionContext"] as ISessionContext;

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

                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> SignoutAsync(IDictionary<string, object> userContext)
        {
            try
            {
                var sessionContext = userContext["SessionContext"] as ISessionContext;

                if (sessionContext == null || sessionContext.CurrentUser == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var currentUser = sessionContext.CurrentUser;

                var result = await _loginManager.LogoutAsync(currentUser.UserIdentityId, currentUser.AuthenticationToken);

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
                var userContext = context.UserContext["SessionContext"] as ISessionContext;

                model.UserPhotoType = UserPhotoTypeEnum.Avatar;
                var result = await _userManager.UpdateAvatarAsync(model, userContext.CurrentUser.Id);

                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

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
                var userContext = context.UserContext["SessionContext"] as ISessionContext;

                model.UserPhotoType = UserPhotoTypeEnum.Cover;
                var result = await _userManager.UpdateCoverAsync(model, userContext.CurrentUser.Id);
                
                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

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
                var userContext = context.UserContext["SessionContext"] as ISessionContext;

                var result = await _userManager.DeleteUserPhotoAsync(userContext.CurrentUser.Id, UserPhotoTypeEnum.Avatar);

                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

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
                var userContext = context.UserContext["SessionContext"] as ISessionContext;

                var result = await _userManager.DeleteUserPhotoAsync(userContext.CurrentUser.Id, UserPhotoTypeEnum.Cover);

                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> UpdateIdentifierAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var user = context.GetArgument<ApplicationUser>("user");
                var userContext = context.UserContext["SessionContext"] as ISessionContext;

                var currentUser = userContext.CurrentUser;
                user.Id = currentUser.Id;

                var result = await _userManager.UpdateIdentifierAsync(user, currentUser.UserIdentityId, currentUser.AuthenticationToken);

                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

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
                var model = context.GetArgument<UserPasswordUpdateDto>("criterias");
                var userContext = context.UserContext["SessionContext"] as ISessionContext;

                var currentUser = userContext.CurrentUser;
                model.UserId = currentUser.Id;

                if (!model.NewPassword.Equals(model.ConfirmPassword))
                {
                    throw new ArgumentException($"{nameof(model.NewPassword)} and {nameof(model.ConfirmPassword)} is not the same");
                }

                var result = await _userManager.ChangePasswordAsync(currentUser.Id, model.CurrentPassword, model.NewPassword);

                if (!result.IsSucceed)
                {
                    HandleContextError(context, result.Errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ex.Message, ex);
            }
        }

        #region Privates
        private UpdateUserPhotoDto GenerateUserPhotoModel(ResolveFieldContext<object> context)
        {
            var model = context.GetArgument<UpdateUserPhotoDto>("criterias");
            var userContext = context.UserContext["SessionContext"] as ISessionContext;

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
