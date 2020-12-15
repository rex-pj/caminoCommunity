using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Media.GraphQL.Resolvers.Contracts;
using Module.Api.Media.Models;

namespace Module.Api.Media.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class ImageMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public CommonResult UpdateAvatarAsync([Service] IImageResolver imageResolver, ImageValidationModel criterias)
        {
            return imageResolver.ValidateImageUrl(criterias);
        }
    }
}
