using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Coco.Api.Framework.GraphQLTypes.ResultTypes
{
    public class SelectOptionType : ObjectGraphType<SelectOption>
    {
        public SelectOptionType()
        {
            Field(x => x.Id, type: (typeof(StringGraphType)));
            Field(x => x.Text, type: (typeof(StringGraphType)));
            Field(x => x.IsSelected, type: (typeof(BooleanGraphType)));
        }
    }
}
