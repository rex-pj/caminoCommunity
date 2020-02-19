using Coco.Api.Framework.GraphQLTypes.Redefines;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class UpdatePerItemInputType : InputObjectGraphType<UpdatePerItemModel>
    {
        public UpdatePerItemInputType()
        {
            Field(x => x.Key, false, typeof(StringGraphType));
            Field(x => x.PropertyName, false, typeof(StringGraphType));
            Field(x => x.Value, false, typeof(DynamicGraphType));
            Field(x => x.Type, false, typeof(IntGraphType));
            Field(x => x.CanEdit, false, typeof(BooleanGraphType));
        }
    }
}
