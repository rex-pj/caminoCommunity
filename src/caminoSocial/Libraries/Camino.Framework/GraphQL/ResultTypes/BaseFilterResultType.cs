using Camino.Framework.Models;
using HotChocolate.Types;

namespace Camino.Framework.GraphQL.ResultTypes
{
    public class BaseFilterResultType : ObjectType<BaseFilterModel>
    {
        protected override void Configure(IObjectTypeDescriptor<BaseFilterModel> descriptor)
        {
            descriptor.Field(x => x.Page).Type<IntType>();
            descriptor.Field(x => x.PageSize).Type<IntType>();
            descriptor.Field(x => x.Search).Type<StringType>();
        }
    }
}
