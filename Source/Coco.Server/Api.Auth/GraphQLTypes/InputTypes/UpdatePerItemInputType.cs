using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.InputTypes
{
    public class UpdatePerItemInputType : InputObjectGraphType<UpdatePerItemModel>
    {
        public UpdatePerItemInputType()
        {
            Field(x => x.Key, false, typeof(StringGraphType));
            Field(x => x.PropertyName, false, typeof(StringGraphType));
            Field(x => x.Value, false, typeof(StringGraphType));
            Field(x => x.Type, false, typeof(IntGraphType));
            Field(x => x.CanEdit, false, typeof(BooleanGraphType));
        }
    }
}
