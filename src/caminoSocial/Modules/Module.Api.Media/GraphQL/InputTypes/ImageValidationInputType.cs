using Module.Api.Media.Models;
using HotChocolate.Types;

namespace Module.Api.Media.GraphQL.InputTypes
{
    public class ImageValidationInputType : InputObjectType<ImageValidationModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ImageValidationModel> descriptor)
        {
            descriptor.Field(x => x.Url).Type<StringType>();
        }
    }
}
