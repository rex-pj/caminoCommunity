using Module.Api.Auth.Models;
using Camino.Framework.GraphQL.Resolvers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using System.Linq;
using System.Security.Claims;
using Camino.Application.Contracts.AppServices.Users;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Identity.Core;
using Camino.Shared.Enums;
using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoAppService _userPhotoAppService;
        private readonly IUserManager<ApplicationUser> _userManager;

        public UserPhotoResolver(IUserPhotoAppService userPhotoAppService, IUserManager<ApplicationUser> userManager)
            : base()
        {
            _userManager = userManager;
            _userPhotoAppService = userPhotoAppService;
        }

        public async Task<UserAvatarModel> GetUserAvatar(ClaimsPrincipal claimsPrincipal, FindUserModel criterias)
        {
            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var userId = currentUserId;
            if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            var photo = await GetUserPhotoÁync(userId, UserPictureTypes.Avatar);
            return new UserAvatarModel
            {
                Code = photo.Code,
                TypeId = photo.TypeId.ToString(),
                Url = photo.Url
            };
        }

        public async Task<UserCoverModel> GetUserCover(ClaimsPrincipal claimsPrincipal, FindUserModel criterias)
        {
            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var userId = currentUserId;
            if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            var photo = await GetUserPhotoÁync(userId, UserPictureTypes.Cover);
            return new UserCoverModel
            {
                Code = photo.Code,
                TypeId = photo.TypeId.ToString(),
                Url = photo.Url
            };
        }

        public async Task<IList<UserPhotoModel>> GetUserPhotos(ClaimsPrincipal claimsPrincipal, FindUserModel criterias)
        {
            var currentUserId = GetCurrentUserId(claimsPrincipal);
            var userId = currentUserId;
            if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            return await GetUserPhotos(userId);
        }

        private async Task<IList<UserPhotoModel>> GetUserPhotos(long userId)
        {
            var userPhotos = await _userPhotoAppService.GetUserPhotosAsync(userId);
            return userPhotos.Select(x => new UserPhotoModel
            {
                Code = x.Code,
                Id = x.Id,
                PhotoType = (UserPictureTypes)x.TypeId
            }).ToList();
        }

        private async Task<UserPhotoResult> GetUserPhotoÁync(long userId, UserPictureTypes type)
        {
            var userPhoto = await _userPhotoAppService.GetByUserIdAsync(userId, type);
            if (userPhoto != null)
            {
                userPhoto.Url = userPhoto.Code;
            }

            return userPhoto;
        }
    }
}
