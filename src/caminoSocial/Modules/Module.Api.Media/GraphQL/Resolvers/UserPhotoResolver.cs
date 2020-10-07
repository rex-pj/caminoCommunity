using Module.Api.Media.Models;
using Module.Api.Media.GraphQL.Resolvers.Contracts;
using Camino.Data.Enums;
using Camino.Framework.Models;
using Camino.Framework.GraphQL.Resolvers;
using System;
using System.Threading.Tasks;
using Camino.IdentityManager.Contracts.Core;
using Camino.Service.Business.Users.Contracts;
using Camino.Service.Projections.Request;

namespace Module.Api.Media.GraphQL.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;

        public UserPhotoResolver(IUserPhotoBusiness userPhotoBusiness, SessionState sessionState)
            : base(sessionState)
        {
            _userPhotoBusiness = userPhotoBusiness;
        }

        public async Task<ICommonResult> UpdateAvatarAsync(UserPhotoUpdateRequest criterias)
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

        public async Task<ICommonResult> UpdateCoverAsync(UserPhotoUpdateRequest criterias)
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
                return CommonResult.Success(new UserPhotoUpdateRequest());
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
                return CommonResult.Success(new UserPhotoUpdateRequest());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
