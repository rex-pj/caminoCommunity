using Module.Api.Content.Models;
using HotChocolate.Types;

namespace Module.Api.Content.GraphQLTypes.InputTypes
{
    public class ImageValidationInputType : InputObjectType<ImageValidationModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ImageValidationModel> descriptor)
        {
            descriptor.Field(x => x.Url).Type<StringType>();
        }
    }
}
