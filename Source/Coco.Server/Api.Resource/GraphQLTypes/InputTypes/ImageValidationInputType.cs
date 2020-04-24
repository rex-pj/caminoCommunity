using Api.Resource.Models;
using HotChocolate.Types;

namespace Api.Resource.GraphQLTypes.InputTypes
{
    public class ImageValidationInputType : InputObjectType<ImageValidationModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ImageValidationModel> descriptor)
        {
            descriptor.Field(x => x.Url).Type<StringType>();
        }
    }
}
