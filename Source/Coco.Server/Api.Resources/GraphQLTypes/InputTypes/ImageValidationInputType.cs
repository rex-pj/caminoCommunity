using Api.Resources.Models;
using HotChocolate.Types;

namespace Api.Resources.GraphQLTypes.InputTypes
{
    public class ImageValidationInputType : InputObjectType<ImageValidationModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ImageValidationModel> descriptor)
        {
            descriptor.Field(x => x.Url).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.IsValid).Ignore();
        }
    }
}
