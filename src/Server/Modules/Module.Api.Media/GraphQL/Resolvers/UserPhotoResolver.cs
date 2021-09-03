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
using System.Security.Claims;

namespace Module.Api.Media.GraphQL.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoService _userPhotoService;

        public UserPhotoResolver(IUserPhotoService userPhotoService)
            : base()
        {
            _userPhotoService = userPhotoService;
        }

        public async Task<CommonResult> UpdateAvatarAsync(ClaimsPrincipal claimsPrincipal, UserPhotoUpdateModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var currentUserId = GetCurrentUserId(claimsPrincipal);
                var result = await _userPhotoService.UpdateUserPhotoAsync(new UserPhotoUpdateRequest
                {
                    PhotoUrl = criterias.PhotoUrl,
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    UserPhotoTypeId = (int)UserPictureType.Avatar,
                    XAxis = criterias.XAxis,
                    YAxis = criterias.YAxis
                }, currentUserId);

                return CommonResult.Success(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommonResult> UpdateCoverAsync(ClaimsPrincipal claimsPrincipal, UserPhotoUpdateModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var currentUserId = GetCurrentUserId(claimsPrincipal);
                var result = await _userPhotoService.UpdateUserPhotoAsync(new UserPhotoUpdateRequest
                {
                    PhotoUrl = criterias.PhotoUrl,
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    XAxis = criterias.XAxis,
                    YAxis = criterias.YAxis,
                    UserPhotoTypeId = (int)UserPictureType.Cover
                }, currentUserId);

                return CommonResult.Success(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommonResult> DeleteAvatarAsync(ClaimsPrincipal claimsPrincipal, PhotoDeleteModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var currentUserId = GetCurrentUserId(claimsPrincipal);
                await _userPhotoService.DeleteUserPhotoAsync(currentUserId, UserPictureType.Avatar);
                return CommonResult.Success(new UserPhotoUpdateRequest());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommonResult> DeleteCoverAsync(ClaimsPrincipal claimsPrincipal, PhotoDeleteModel criterias)
        {
            try
            {
                if (!criterias.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var currentUserId = GetCurrentUserId(claimsPrincipal);
                await _userPhotoService.DeleteUserPhotoAsync(currentUserId, UserPictureType.Cover);
                return CommonResult.Success(new UserPhotoUpdateRequest());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
