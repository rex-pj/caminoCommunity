using Module.Api.Media.Models;
using Camino.Framework.Models;

namespace Module.Api.Media.GraphQL.Resolvers.Contracts
{
    public interface IImageResolver
    {
        CommonResult ValidateImageUrl(ImageValidationModel criterias);
    }
}
