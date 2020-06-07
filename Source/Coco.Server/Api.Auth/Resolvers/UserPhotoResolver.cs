using Api.Auth.Resolvers.Contracts;
using Coco.Auth.Models;
using Coco.Business.Contracts;
using Coco.Common.Const;
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

        public UserPhotoResolver(IUserPhotoBusiness userPhotoBusiness, IUserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _userPhotoBusiness = userPhotoBusiness;
        }

        public async Task<string> GetAvatarUrlByUserId(IResolverContext context)
        {
            var criterias = context.Argument<FindUserModel>("criterias");
            var currentUser = context.ContextData[SessionContextConst.CURRENT_USER] as ApplicationUser;
            var userId = currentUser.Id;
            if (criterias != null && !string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            return GetUserPhotoUrl(userId, UserPhotoTypeEnum.Avatar);
        }

        public async Task<string> GetCoverUrlByUserId(IResolverContext context)
        {
            var criterias = context.Argument<FindUserModel>("criterias");
            var currentUser = context.ContextData[SessionContextConst.CURRENT_USER] as ApplicationUser;
            var userId = currentUser.Id;
            if (criterias != null &&  !string.IsNullOrEmpty(criterias.UserId))
            {
                userId = await _userManager.DecryptUserIdAsync(criterias.UserId);
            }

            return GetUserPhotoUrl(userId, UserPhotoTypeEnum.Cover);
        }

        private string GetUserPhotoUrl(long userId, UserPhotoTypeEnum type)
        {
            var userPhoto = _userPhotoBusiness.GetUserPhotoByUserIdAsync(userId, type);
            if (userPhoto == null)
            {
                return string.Empty;
            }

            return userPhoto.Code;
        }
    }
}
