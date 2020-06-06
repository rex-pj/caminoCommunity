using Api.Auth.Resolvers.Contracts;
using Coco.Business.Contracts;
using Coco.Common.Const;
using Coco.Entities.Enums;
using Coco.Framework.Models;
using HotChocolate.Resolvers;

namespace Api.Auth.Resolvers
{
    public class UserPhotoResolver : IUserPhotoResolver
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;
        public UserPhotoResolver(IUserPhotoBusiness userPhotoBusiness)
        {
            _userPhotoBusiness = userPhotoBusiness;
        }

        public string GetAvatarUrlByUserId(IResolverContext context)
        {
            var currentUser = context.ContextData[SessionContextConst.CURRENT_USER] as ApplicationUser;
            return GetUserPhotoUrl(currentUser, UserPhotoTypeEnum.Avatar);
        }

        public string GetCoverUrlByUserId(IResolverContext context)
        {
            var currentUser = context.ContextData[SessionContextConst.CURRENT_USER] as ApplicationUser;
            return GetUserPhotoUrl(currentUser, UserPhotoTypeEnum.Cover);
        }

        private string GetUserPhotoUrl(ApplicationUser user, UserPhotoTypeEnum type)
        {
            var userPhoto = _userPhotoBusiness.GetUserPhotoByUserId(user.Id, type);
            if (userPhoto == null)
            {
                return string.Empty;
            }

            return userPhoto.Code;
        }
    }
}
