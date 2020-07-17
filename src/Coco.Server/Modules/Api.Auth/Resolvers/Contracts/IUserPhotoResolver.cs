using Api.Auth.Models;
using Coco.Auth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<UserAvatarModel> GetUserAvatar(FindUserModel criterias);
        Task<UserCoverModel> GetUserCover(FindUserModel criterias);
        Task<IEnumerable<UserPhotoModel>> GetUserPhotos(FindUserModel criterias);
    }
}
