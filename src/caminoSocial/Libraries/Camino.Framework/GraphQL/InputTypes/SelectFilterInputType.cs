using Camino.Framework.Models;
using HotChocolate.Types;

namespace Camino.Framework.GraphQL.InputTypes
{
    public class SelectFilterInputType : InputObjectType<SelectFilterModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SelectFilterModel> descriptor)
        {
            descriptor.Field(x => x.Query).Type<StringType>();
            descriptor.Field(x => x.CurrentId).Type<LongType>();
            descriptor.Field(x => x.IsParentOnly).Type<BooleanType>();
        }
    }
}
