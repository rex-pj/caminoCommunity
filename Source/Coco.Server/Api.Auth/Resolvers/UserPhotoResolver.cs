using Api.Auth.Models;
using Api.Auth.Resolvers.Contracts;
using AutoMapper;
using Coco.Auth.Models;
using Coco.Business.Contracts;
using Coco.Entities.Dtos;
using Coco.Entities.Enums;
using Coco.Framework.Models;
using Coco.Framework.Resolvers;
using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.SessionManager.Core;
using HotChocolate.Resolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserPhotoResolver(IUserPhotoBusiness userPhotoBusiness, IUserManager<ApplicationUser> userManager, IMapper mapper, SessionState sessionState) 
            : base(sessionState)
        {
            _userManager = userManager;
            _userPhotoBusiness = userPhotoBusiness;
            _mapper = mapper;
        }

        public async Task<UserAvatarModel> GetUserAvatar(FindUserModel criterias)
        {
            if (CurrentUser != null)
            {
                var userId = CurrentUser.Id;
                if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
                {
                    userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
                }

                return _mapper.Map<UserAvatarModel>(GetUserPhoto(userId, UserPhotoTypeEnum.Avatar));
            }
            return null;
        }

        public async Task<UserCoverModel> GetUserCover(FindUserModel criterias)
        {
            if (CurrentUser != null)
            {
                var userId = CurrentUser.Id;
                if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
                {
                    userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
                }

                return _mapper.Map<UserCoverModel>(GetUserPhoto(userId, UserPhotoTypeEnum.Cover));
            }
            return null;
        }

        public async Task<IEnumerable<UserPhotoModel>> GetUserPhotos(FindUserModel criterias)
        {
            if (CurrentUser != null)
            {
                var userId = CurrentUser.Id;
                if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
                {
                    userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
                }

                var userPhotos = await GetUserPhotos(userId);
                return _mapper.Map<IEnumerable<UserPhotoModel>>(userPhotos);
            }
            return new List<UserPhotoModel>();
        }

        private async Task<IEnumerable<UserPhotoModel>> GetUserPhotos(long userId)
        {
            var userPhotos = await _userPhotoBusiness.GetUserPhotosAsync(userId);
            return _mapper.Map<IEnumerable<UserPhotoModel>>(userPhotos);
        }

        private UserPhotoDto GetUserPhoto(long userId, UserPhotoTypeEnum type)
        {
            var userPhoto = _userPhotoBusiness.GetUserPhotoByUserId(userId, type);
            if (userPhoto != null)
            {
                userPhoto.Url = userPhoto.Code;
            }

            return userPhoto;
        }
    }
}
