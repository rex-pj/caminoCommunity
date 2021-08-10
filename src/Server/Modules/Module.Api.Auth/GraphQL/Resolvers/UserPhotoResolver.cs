using Module.Api.Auth.Models;
using Camino.Shared.Enums;
using Camino.Framework.GraphQL.Resolvers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.Core.Domain.Identities;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Camino.Shared.Results.Media;
using Camino.Core.Contracts.Services.Users;
using Camino.Core.Contracts.IdentityManager;
using System.Linq;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoService _userPhotoService;
        private readonly IUserManager<ApplicationUser> _userManager;

        public UserPhotoResolver(IUserPhotoService userPhotoService, IUserManager<ApplicationUser> userManager,
            ISessionContext sessionContext)
            : base(sessionContext)
        {
            _userManager = userManager;
            _userPhotoService = userPhotoService;
        }

        public async Task<UserAvatarModel> GetUserAvatar(ApplicationUser currentUser, FindUserModel criterias)
        {
            if (currentUser == null)
            {
                return null;
            }

            var userId = currentUser.Id;
            if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            var photo = GetUserPhoto(userId, UserPictureType.Avatar);
            return new UserAvatarModel
            {
                Code = photo.Code,
                TypeId = photo.TypeId.ToString(),
                Url = photo.Url
            };
        }

        public async Task<UserCoverModel> GetUserCover(ApplicationUser currentUser, FindUserModel criterias)
        {
            if (currentUser == null)
            {
                return null;
            }

            var userId = currentUser.Id;
            if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            var photo = GetUserPhoto(userId, UserPictureType.Cover);
            return new UserCoverModel
            {
                Code = photo.Code,
                TypeId = photo.TypeId.ToString(),
                Url = photo.Url
            };
        }

        public async Task<IList<UserPhotoModel>> GetUserPhotos(ApplicationUser currentUser, FindUserModel criterias)
        {
            if (currentUser == null)
            {
                return new List<UserPhotoModel>();
            }

            var userId = currentUser.Id;
            if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            return await GetUserPhotos(userId);
        }

        private async Task<IList<UserPhotoModel>> GetUserPhotos(long userId)
        {
            var userPhotos = await _userPhotoService.GetUserPhotosAsync(userId);
            return userPhotos.Select(x => new UserPhotoModel
            {
                Code = x.Code,
                Id = x.Id,
                PhotoType = (UserPictureType)x.TypeId
            }).ToList();
        }

        private UserPhotoResult GetUserPhoto(long userId, UserPictureType type)
        {
            var userPhoto = _userPhotoService.GetUserPhotoByUserId(userId, type);
            if (userPhoto != null)
            {
                userPhoto.Url = userPhoto.Code;
            }

            return userPhoto;
        }
    }
}
