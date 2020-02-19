using Coco.Api.Framework.GraphQLTypes.Redefines;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class ApiItemUpdatedResultType : ApiResultType<UpdatePerItemModel, ItemUpdatedResultType>
    {

    }

    public class ItemUpdatedResultType : ObjectGraphType<UpdatePerItemModel>
    {
        public ItemUpdatedResultType()
        {
            Field(x => x.Key, false, typeof(StringGraphType));
            Field(x => x.PropertyName, false, typeof(StringGraphType));
            Field(x => x.Value, false, typeof(DynamicGraphType));
            Field(x => x.Type, false, typeof(IntGraphType));
        }
    }
}
