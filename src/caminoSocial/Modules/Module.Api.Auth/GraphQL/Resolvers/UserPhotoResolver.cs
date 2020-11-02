using Module.Api.Auth.Models;
using AutoMapper;
using Camino.Data.Enums;
using Camino.Framework.GraphQL.Resolvers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Camino.IdentityManager.Contracts;
using Camino.IdentityManager.Models;
using Camino.IdentityManager.Contracts.Core;
using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Camino.Service.Business.Users.Contracts;
using Camino.Service.Projections.Media;
using HotChocolate.Resolvers;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class UserPhotoResolver : BaseResolver, IUserPhotoResolver
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserPhotoResolver(IUserPhotoBusiness userPhotoBusiness, IUserManager<ApplicationUser> userManager, IMapper mapper, 
            ISessionContext sessionContext) 
            : base(sessionContext)
        {
            _userManager = userManager;
            _userPhotoBusiness = userPhotoBusiness;
            _mapper = mapper;
        }

        public async Task<UserAvatarModel> GetUserAvatar(ApplicationUser currentUser, FindUserModel criterias)
        {
            if (currentUser != null)
            {
                var userId = currentUser.Id;
                if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
                {
                    userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
                }

                return _mapper.Map<UserAvatarModel>(GetUserPhoto(userId, UserPhotoKind.Avatar));
            }
            return null;
        }

        public async Task<UserCoverModel> GetUserCover(ApplicationUser currentUser, FindUserModel criterias)
        {
            if (currentUser != null)
            {
                var userId = currentUser.Id;
                if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
                {
                    userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
                }

                return _mapper.Map<UserCoverModel>(GetUserPhoto(userId, UserPhotoKind.Cover));
            }
            return null;
        }

        public async Task<IEnumerable<UserPhotoModel>> GetUserPhotos(ApplicationUser currentUser, FindUserModel criterias)
        {
            if (currentUser != null)
            {
                var userId = currentUser.Id;
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

        private UserPhotoProjection GetUserPhoto(long userId, UserPhotoKind type)
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
