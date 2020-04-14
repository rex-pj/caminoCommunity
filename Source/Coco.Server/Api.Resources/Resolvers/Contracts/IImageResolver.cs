using Api.Resources.Models;
using HotChocolate.Resolvers;

namespace Api.Public.Resolvers.Contracts
{
    public interface IImageResolver
    {
        ImageValidationModel ValidateImageUrl(IResolverContext context);
    }
}
