using Camino.Core.Domain.Identities;
using Module.Api.Auth.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<UserAvatarModel> GetUserAvatar(ApplicationUser currentUser, FindUserModel criterias);
        Task<UserCoverModel> GetUserCover(ApplicationUser currentUser, FindUserModel criterias);
        Task<IList<UserPhotoModel>> GetUserPhotos(ApplicationUser currentUser, FindUserModel criterias);
    }
}
