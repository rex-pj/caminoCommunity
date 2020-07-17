using Coco.Api.Content.Models;
using Coco.Framework.Models;

namespace Coco.Api.Content.Resolvers.Contracts
{
    public interface IImageResolver
    {
        ICommonResult ValidateImageUrl(ImageValidationModel criterias);
    }
}
