using Api.Resources.Models;
using HotChocolate.Types;

namespace Api.Public.GraphQLTypes.ResultTypes
{
    public class ImageValidationResultType : ObjectType<ImageValidationModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ImageValidationModel> descriptor)
        {
            descriptor.Field(x => x.IsValid).Type<BooleanType>();
            descriptor.Field(x => x.Url).Ignore();
        }
    }
}
