using Camino.Framework.Models;
using HotChocolate.Types;

namespace Camino.Framework.GraphQL.InputTypes
{
    public class PictureRequestInputType : InputObjectType<PictureRequestModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PictureRequestModel> descriptor)
        {
            descriptor.Field(x => x.Base64Data).Type<StringType>();
            descriptor.Field(x => x.ContentType).Type<StringType>();
            descriptor.Field(x => x.FileName).Type<StringType>();
            descriptor.Field(x => x.Id).Type<LongType>();
        }
    }
}

