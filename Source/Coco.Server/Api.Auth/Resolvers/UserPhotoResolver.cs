using Api.Auth.Resolvers.Contracts;
using Coco.Business.Contracts;
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
        public UserPhotoResolver(IUserPhotoBusiness userPhotoBusiness)
        {
            _userPhotoBusiness = userPhotoBusiness;
        }

        public async Task<string> GetAvatarUrlByUserIdAsync(IResolverContext context)
        {
            var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
            var currentUser = await sessionContext.GetCurrentUserAsync();
            return GetUserPhotoUrl(currentUser, UserPhotoTypeEnum.Avatar);
        }

        public async Task<string> GetCoverUrlByUserIdAsync(IResolverContext context)
        {
            var sessionContext = context.ContextData["SessionContext"] as ISessionContext;
            var currentUser = await sessionContext.GetCurrentUserAsync();
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
