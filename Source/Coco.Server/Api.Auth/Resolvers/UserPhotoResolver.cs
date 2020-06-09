using Api.Auth.Models;
using Api.Auth.Resolvers.Contracts;
using AutoMapper;
using Coco.Auth.Models;
using Coco.Business.Contracts;
using Coco.Common.Const;
using Coco.Entities.Dtos;
using Coco.Entities.Enums;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using HotChocolate.Resolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers
{
    public class UserPhotoResolver : IUserPhotoResolver
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ISessionContext _sessionContext;

        public UserPhotoResolver(IUserPhotoBusiness userPhotoBusiness, IUserManager<ApplicationUser> userManager, ISessionContext sessionContext, IMapper mapper)
        {
            _userManager = userManager;
            _userPhotoBusiness = userPhotoBusiness;
            _mapper = mapper;
            _sessionContext = sessionContext;
        }

        public async Task<UserAvatarModel> GetUserAvatar(IResolverContext context)
        {
            var criterias = context.Argument<FindUserModel>("criterias");
            var currentUser = context.ContextData[SessionContextConst.CURRENT_USER] as ApplicationUser;
            if (currentUser != null)
            {
                var userId = currentUser.Id;
                if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
                {
                    userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
                }

                return _mapper.Map<UserAvatarModel>(GetUserPhoto(userId, UserPhotoTypeEnum.Avatar));
            }
            return null;
        }

        public async Task<UserCoverModel> GetUserCover(IResolverContext context)
        {
            var criterias = context.Argument<FindUserModel>("criterias");
            var currentUser = context.ContextData[SessionContextConst.CURRENT_USER] as ApplicationUser;
            if (currentUser != null)
            {
                var userId = currentUser.Id;
                if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
                {
                    userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
                }

                return _mapper.Map<UserCoverModel>(GetUserPhoto(userId, UserPhotoTypeEnum.Cover));
            }
            return null;
        }

        public async Task<IEnumerable<UserPhotoModel>> GetUserPhotos(IResolverContext context)
        {
            var criterias = context.Argument<FindUserModel>("criterias");
            var currentUser = context.ContextData[SessionContextConst.CURRENT_USER] as ApplicationUser;
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
