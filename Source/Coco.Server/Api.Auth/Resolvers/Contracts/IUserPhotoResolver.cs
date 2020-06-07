using HotChocolate.Resolvers;
using System.Threading.Tasks;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        Task<string> GetAvatarUrlByUserId(IResolverContext context);
        Task<string> GetCoverUrlByUserId(IResolverContext context);
    }
}
