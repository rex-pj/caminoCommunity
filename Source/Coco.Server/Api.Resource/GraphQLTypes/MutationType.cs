using Api.Public.Resolvers.Contracts;
using Api.Resource.GraphQLTypes.InputTypes;
using Coco.Framework.GraphQLTypes.ResultTypes;
using HotChocolate.Types;

namespace Api.Resource.GraphQLTypes
{
    public class MutationType : ObjectType
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
