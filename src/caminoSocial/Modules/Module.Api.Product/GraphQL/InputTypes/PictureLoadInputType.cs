using HotChocolate.Types;
using Module.Api.Product.Models;

namespace Module.Api.Product.GraphQL.InputTypes
{
    public class PictureLoadInputType : InputObjectType<PictureLoadModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PictureLoadModel> descriptor)
        {
            descriptor.Field(x => x.Base64Data).Type<StringType>();
            descriptor.Field(x => x.ContentType).Type<StringType>();
            descriptor.Field(x => x.FileName).Type<StringType>();
            descriptor.Field(x => x.BinaryData).Ignore();
            descriptor.Field(x => x.Id).Ignore();
        }
    }
}

