using Module.Api.Content.Models;
using Camino.Framework.Models;

namespace Module.Api.Content.Resolvers.Contracts
{
    public interface IImageResolver
    {
        ICommonResult ValidateImageUrl(ImageValidationModel criterias);
    }
}
