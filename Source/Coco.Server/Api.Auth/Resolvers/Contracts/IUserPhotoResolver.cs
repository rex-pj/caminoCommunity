using Api.Auth.Models;
using HotChocolate.Resolvers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<UserAvatarModel> GetUserAvatar(IResolverContext context);
        Task<UserCoverModel> GetUserCover(IResolverContext context);
        Task<IEnumerable<UserPhotoModel>> GetUserPhotos(IResolverContext context);
    }
}
