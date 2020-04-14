using Api.Public.GraphQLTypes.ResultTypes;
using Api.Public.Resolvers.Contracts;
using Api.Resources.GraphQLTypes.InputTypes;
using HotChocolate.Types;

namespace Api.Resources.QueryTypes
{
    public class ImageQueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IImageResolver>(x => x.ValidateImageUrl(default))
                .Type<ImageValidationResultType>()
                .Argument("criterias", a => a.Type<ImageValidationInputType>())
                .Resolver(ctx => ctx.Service<IImageResolver>().ValidateImageUrl(ctx));
        }
    }
}
