using Api.Public.Resolvers.Contracts;
using Api.Resources.GraphQLTypes.InputTypes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Resources.MutationTypes
{
    public class ImageMutationType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IImageResolver>(x => x.ValidateImageUrl(default))
                .Type<ApiResultType>()
                .Argument("criterias", a => a.Type<ImageValidationInputType>())
                .Resolver(ctx => ctx.Service<IImageResolver>().ValidateImageUrl(ctx));
        }
    }
}
