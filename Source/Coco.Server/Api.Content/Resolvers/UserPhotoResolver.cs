using Api.Content.Models;
using Api.Content.Resolvers.Contracts;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.General;
using Coco.Entities.Enums;
using Coco.Framework.Models;
using Coco.Framework.Resolvers;
using Coco.Framework.SessionManager.Contracts;
using HotChocolate.Resolvers;
using System;
using System.Threading.Tasks;

namespace Api.Content.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IUserPhotoBusiness _userPhotoBusiness;

        public UserPhotoResolver(IUserManager<ApplicationUser> userManager, IUserPhotoBusiness userPhotoBusiness)
        {
            _userManager = userManager;
            _userPhotoBusiness = userPhotoBusiness;
        }

        public async Task<ICommonResult> UpdateAvatarAsync(IResolverContext context)
        {
            try
            {
                var model = GenerateUserPhotoModel(context);
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                var currentUser = await sessionContext.GetCurrentUserAsync();

                var user = await _userManager.FindByIdAsync(currentUser.Id.ToString());
                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }

                model.UserPhotoType = UserPhotoTypeEnum.Avatar;
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(model, user.Id);

                return CommonResult.Success(result);
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<ICommonResult> UpdateCoverAsync(IResolverContext context)
        {
            try
            {
                var model = GenerateUserPhotoModel(context);
                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                var currentUser = await sessionContext.GetCurrentUserAsync();

                var user = await _userManager.FindByIdAsync(currentUser.Id.ToString());
                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }

                model.UserPhotoType = UserPhotoTypeEnum.Cover;
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(model, user.Id);

                return CommonResult.Success(result);
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<ICommonResult> DeleteAvatarAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<PhotoDeleteModel>("criterias");
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                var currentUser = sessionContext.GetCurrentUserAsync();
                if (currentUser.Id < 0)
                {
                    throw new ArgumentException(nameof(currentUser.Id));
                }

                await _userPhotoBusiness.DeleteUserPhotoAsync(currentUser.Id, UserPhotoTypeEnum.Avatar);
                return CommonResult.Success(new UserPhotoUpdateDto());
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
            }
        }

        public async Task<ICommonResult> DeleteCoverAsync(IResolverContext context)
        {
            try
            {
                var criterias = context.Argument<PhotoDeleteModel>("criterias");

                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
                var currentUser = await sessionContext.GetCurrentUserAsync();
                await _userPhotoBusiness.DeleteUserPhotoAsync(currentUser.Id, UserPhotoTypeEnum.Cover);
                return CommonResult.Success(new UserPhotoUpdateDto());
            }
            catch (Exception ex)
            {
                HandleContextError(context, ex);
                return null;
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
