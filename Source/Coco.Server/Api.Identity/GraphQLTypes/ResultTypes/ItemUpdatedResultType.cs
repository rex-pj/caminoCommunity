using Coco.Api.Framework.Models;
using HotChocolate.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class ItemUpdatedResultType : ObjectType<UpdatePerItemModel>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdatePerItemModel> descriptor)
        {
            descriptor.Field(x => x.Key).Type<StringType>();
            descriptor.Field(x => x.PropertyName).Type<StringType>();
            descriptor.Field(x => x.Value).Type<AnyType>();
        }
    }
}
