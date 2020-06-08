using Api.Auth.Models;
using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<UserAvatarModel> GetUserAvatarUrl(IResolverContext context);
        Task<UserCoverModel> GetUserCoverUrl(IResolverContext context);
    }
}
