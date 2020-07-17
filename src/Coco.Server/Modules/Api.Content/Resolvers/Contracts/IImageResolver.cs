using Api.Content.Models;
using Coco.Framework.Models;

namespace Api.Content.Resolvers.Contracts
{
    public interface IImageResolver
    {
        ICommonResult ValidateImageUrl(ImageValidationModel criterias);
    }
}
