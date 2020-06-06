using HotChocolate.Resolvers;

namespace Api.Auth.Resolvers.Contracts
{
    public interface IUserPhotoResolver
    {
        string GetAvatarUrlByUserId(IResolverContext context);
        string GetCoverUrlByUserId(IResolverContext context);
    }
}
