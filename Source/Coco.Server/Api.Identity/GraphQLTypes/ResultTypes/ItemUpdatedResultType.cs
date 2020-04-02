using Coco.Api.Framework.GraphQLTypes.Redefines;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using HotChocolate.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class ApiItemUpdatedResultType : ApiResultType<UpdatePerItemModel, ItemUpdatedResultType>
    {

    }

    public class ItemUpdatedResultType : ObjectType<UpdatePerItemModel>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdatePerItemModel> descriptor)
        {
            descriptor.Field(x => x.Key).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.PropertyName).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Value).Type<NonNullType<DynamicType>>();
            descriptor.Field(x => x.Type).Type<NonNullType<IntType>>();
        }
    }
}
