using Module.Api.Content.Models;
using Module.Api.Content.Resolvers.Contracts;
using Coco.Business.Contracts;
using Coco.Business.Dtos.General;
using Coco.Data.Enums;
using Coco.Framework.Models;
using Coco.Framework.Resolvers;
using Coco.Framework.SessionManager.Core;
using System;
using System.Threading.Tasks;

namespace Module.Api.Content.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;

        public UserPhotoResolver(IUserPhotoBusiness userPhotoBusiness, SessionState sessionState)
            : base(sessionState)
        {
            _userPhotoBusiness = userPhotoBusiness;
        }

        public async Task<ICommonResult> UpdateAvatarAsync(UserPhotoUpdation criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                criterias.UserPhotoType = UserPhotoKind.Avatar;
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(criterias, CurrentUser.Id);

                return CommonResult.Success(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICommonResult> UpdateCoverAsync(UserPhotoUpdation criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                criterias.UserPhotoType = UserPhotoKind.Cover;
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

                await _userPhotoBusiness.DeleteUserPhotoAsync(CurrentUser.Id, UserPhotoKind.Avatar);
                return CommonResult.Success(new UserPhotoUpdation());
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

                await _userPhotoBusiness.DeleteUserPhotoAsync(CurrentUser.Id, UserPhotoKind.Cover);
                return CommonResult.Success(new UserPhotoUpdation());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
