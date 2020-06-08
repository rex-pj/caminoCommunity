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

        public async Task<UserAvatarModel> GetUserAvatarUrl(IResolverContext context)
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

                return _mapper.Map<UserAvatarModel>(GetUserPhotoUrl(userId, UserPhotoTypeEnum.Avatar));
            }
            return null;
        }

        public async Task<UserCoverModel> GetUserCoverUrl(IResolverContext context)
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

                return _mapper.Map<UserCoverModel>(GetUserPhotoUrl(userId, UserPhotoTypeEnum.Cover));
            }
            return null;
        }

        private UserPhotoDto GetUserPhotoUrl(long userId, UserPhotoTypeEnum type)
        {
            var userPhoto = _userPhotoBusiness.GetUserPhotoByUserId(userId, type);

            return userPhoto;
        }
    }
}
