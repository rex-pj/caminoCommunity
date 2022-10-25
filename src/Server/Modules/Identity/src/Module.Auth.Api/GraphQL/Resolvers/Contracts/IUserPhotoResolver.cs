using Module.Auth.Api.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Auth.Api.GraphQL.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<UserAvatarModel> GetUserAvatar(ClaimsPrincipal claimsPrincipal, FindUserModel criterias);
        Task<UserCoverModel> GetUserCover(ClaimsPrincipal claimsPrincipal, FindUserModel criterias);
        Task<IList<UserPhotoModel>> GetUserPhotos(ClaimsPrincipal claimsPrincipal, FindUserModel criterias);
    }
}
