using Module.Api.Media.Models;
using Module.Api.Media.GraphQL.Resolvers.Contracts;
using Camino.Framework.Models;
using Camino.Framework.GraphQL.Resolvers;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Shared.Enums;

namespace Module.Api.Media.GraphQL.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoAppService _userPhotoAppService;

        public UserPhotoResolver(IUserPhotoAppService userPhotoAppService)
            : base()
        {
            _userPhotoAppService = userPhotoAppService;
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
                var result = await _userPhotoAppService.UpdateAsync(new UserPhotoUpdateRequest
                {
                    PhotoUrl = criterias.PhotoUrl,
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    UserPhotoTypeId = (int)UserPictureTypes.Avatar,
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
                var result = await _userPhotoAppService.UpdateAsync(new UserPhotoUpdateRequest
                {
                    PhotoUrl = criterias.PhotoUrl,
                    FileName = criterias.FileName,
                    Width = criterias.Width,
                    Height = criterias.Height,
                    Scale = criterias.Scale,
                    XAxis = criterias.XAxis,
                    YAxis = criterias.YAxis,
                    UserPhotoTypeId = (int)UserPictureTypes.Cover
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
                await _userPhotoAppService.DeleteByUserIdAsync(currentUserId, UserPictureTypes.Avatar);
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
                await _userPhotoAppService.DeleteByUserIdAsync(currentUserId, UserPictureTypes.Cover);
                return CommonResult.Success(new UserPhotoUpdateRequest());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
