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

        public async Task<ICommonResult> UpdateAvatarAsync(UserPhotoUpdateDto criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                criterias.UserPhotoType = UserPhotoTypeEnum.Avatar;
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(criterias, CurrentUser.Id);

                return CommonResult.Success(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICommonResult> UpdateCoverAsync(UserPhotoUpdateDto criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                criterias.UserPhotoType = UserPhotoTypeEnum.Cover;
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(criterias, CurrentUser.Id);

                return CommonResult.Success(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICommonResult> DeleteAvatarAsync(PhotoDeleteModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                await _userPhotoBusiness.DeleteUserPhotoAsync(CurrentUser.Id, UserPhotoTypeEnum.Avatar);
                return CommonResult.Success(new UserPhotoUpdateDto());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICommonResult> DeleteCoverAsync(PhotoDeleteModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                await _userPhotoBusiness.DeleteUserPhotoAsync(CurrentUser.Id, UserPhotoTypeEnum.Cover);
                return CommonResult.Success(new UserPhotoUpdateDto());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
