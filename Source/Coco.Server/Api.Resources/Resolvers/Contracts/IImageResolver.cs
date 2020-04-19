using Coco.Api.Framework.Models;
using HotChocolate.Resolvers;

namespace Api.Public.Resolvers.Contracts
{
    public interface IImageResolver
    {
        IApiResult ValidateImageUrl(IResolverContext context);
    }
}
