using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<string> GetAvatarUrlByUserIdAsync(IResolverContext context);
        Task<string> GetCoverUrlByUserIdAsync(IResolverContext context);
    }
}
