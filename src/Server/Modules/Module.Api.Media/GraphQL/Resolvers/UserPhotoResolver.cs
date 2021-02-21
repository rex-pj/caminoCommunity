using Module.Api.Media.Models;
using Module.Api.Media.GraphQL.Resolvers.Contracts;
using Camino.Shared.Enums;
using Camino.Framework.Models;
using Camino.Framework.GraphQL.Resolvers;
using System;
using System.Threading.Tasks;
using Camino.Core.Contracts.Services.Users;
using Camino.Core.Domain.Identities;
using Camino.Core.Contracts.IdentityManager;
using Camino.Shared.Requests.Identifiers;

namespace Module.Api.Media.GraphQL.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoService _userPhotoService;

        public UserPhotoResolver(IUserPhotoService userPhotoService, ISessionContext sessionContext)
            : base(sessionContext)
        {
            _userPhotoService = userPhotoService;
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
                var result = await _userPhotoService.UpdateUserPhotoAsync(criterias, currentUser.Id);

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
                var result = await _userPhotoService.UpdateUserPhotoAsync(criterias, currentUser.Id);

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

                await _userPhotoService.DeleteUserPhotoAsync(currentUser.Id, UserPhotoKind.Avatar);
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

                await _userPhotoService.DeleteUserPhotoAsync(currentUser.Id, UserPhotoKind.Cover);
                return CommonResult.Success(new UserPhotoUpdateRequest());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
