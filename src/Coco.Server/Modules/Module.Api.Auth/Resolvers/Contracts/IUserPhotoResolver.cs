using Module.Api.Auth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace  Module.Api.Auth.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<UserAvatarModel> GetUserAvatar(FindUserModel criterias);
        Task<UserCoverModel> GetUserCover(FindUserModel criterias);
        Task<IEnumerable<UserPhotoModel>> GetUserPhotos(FindUserModel criterias);
    }
}
