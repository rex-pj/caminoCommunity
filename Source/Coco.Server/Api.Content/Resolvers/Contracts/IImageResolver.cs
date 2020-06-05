using Coco.Framework.Models;
using HotChocolate.Resolvers;

namespace Api.Content.Resolvers.Contracts
{
    public interface IImageResolver
    {
        ICommonResult ValidateImageUrl(IResolverContext context);
    }
}
