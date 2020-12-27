using Camino.IdentityManager.Models;
using HotChocolate.Resolvers;
using Module.Api.Auth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace  Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<UserAvatarModel> GetUserAvatar(ApplicationUser currentUser, FindUserModel criterias);
        Task<UserCoverModel> GetUserCover(ApplicationUser currentUser, FindUserModel criterias);
        Task<IEnumerable<UserPhotoModel>> GetUserPhotos(ApplicationUser currentUser, FindUserModel criterias);
    }
}
