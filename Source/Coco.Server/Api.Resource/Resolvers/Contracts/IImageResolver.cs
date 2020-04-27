using Coco.Framework.Models;
using HotChocolate.Resolvers;

namespace Api.Public.Resolvers.Contracts
{
    public interface IImageResolver
    {
        ICommonResult ValidateImageUrl(IResolverContext context);
    }
}
