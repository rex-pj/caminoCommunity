using Module.Api.Media.Models;
using Module.Api.Media.GraphQL.Resolvers.Contracts;
using Camino.Data.Enums;
using Camino.Framework.Models;
using Camino.Framework.GraphQL.Resolvers;
using System;
using System.Threading.Tasks;
using Camino.Service.Business.Users.Contracts;
using Camino.Service.Projections.Request;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Models;

namespace Module.Api.Media.GraphQL.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;

        public UserPhotoResolver(IUserPhotoBusiness userPhotoBusiness, ISessionContext sessionContext)
            : base(sessionContext)
        {
            _userPhotoBusiness = userPhotoBusiness;
        }

        public async Task<CommonResult> UpdateAvatarAsync(ApplicationUser currentUser, UserPhotoUpdateRequest criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                criterias.UserPhotoType = UserPhotoKind.Avatar;
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(criterias, currentUser.Id);

                return CommonResult.Success(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommonResult> UpdateCoverAsync(ApplicationUser currentUser, UserPhotoUpdateRequest criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                criterias.UserPhotoType = UserPhotoKind.Cover;
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(criterias, currentUser.Id);

                return CommonResult.Success(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommonResult> DeleteAvatarAsync(ApplicationUser currentUser, PhotoDeleteModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                await _userPhotoBusiness.DeleteUserPhotoAsync(currentUser.Id, UserPhotoKind.Avatar);
                return CommonResult.Success(new UserPhotoUpdateRequest());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommonResult> DeleteCoverAsync(ApplicationUser currentUser, PhotoDeleteModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                await _userPhotoBusiness.DeleteUserPhotoAsync(currentUser.Id, UserPhotoKind.Cover);
                return CommonResult.Success(new UserPhotoUpdateRequest());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
