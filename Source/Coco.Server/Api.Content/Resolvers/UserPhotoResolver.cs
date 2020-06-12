using Api.Content.Models;
using Api.Content.Resolvers.Contracts;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.General;
using Coco.Entities.Enums;
using Coco.Framework.Models;
using Coco.Framework.Resolvers;
using Coco.Framework.SessionManager.Core;
using HotChocolate.Resolvers;
using System;
using System.Threading.Tasks;

namespace Api.Content.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;

        public UserPhotoResolver(IUserPhotoBusiness userPhotoBusiness, SessionState sessionState)
            : base(sessionState)
        {
            _userPhotoBusiness = userPhotoBusiness;
        }

        public async Task<ICommonResult> UpdateAvatarAsync(IResolverContext context)
        {
            try
            {
                var model = GenerateUserPhotoModel(context);
                model.UserPhotoType = UserPhotoTypeEnum.Avatar;
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(model, CurrentUser.Id);

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
                model.UserPhotoType = UserPhotoTypeEnum.Cover;
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(model, CurrentUser.Id);

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

                await _userPhotoBusiness.DeleteUserPhotoAsync(CurrentUser.Id, UserPhotoTypeEnum.Avatar);
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

                await _userPhotoBusiness.DeleteUserPhotoAsync(CurrentUser.Id, UserPhotoTypeEnum.Cover);
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
