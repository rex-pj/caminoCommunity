using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class ItemUpdatedResultType : ObjectGraphType<UpdatePerItemModel>
    {
        public ItemUpdatedResultType()
        {
            Field(x => x.Key, false, typeof(StringGraphType));
            Field(x => x.PropertyName, false, typeof(StringGraphType));
            Field(x => x.Value, false, typeof(StringGraphType));
            Field(x => x.Type, false, typeof(IntGraphType));
        }
    }
}
