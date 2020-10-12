using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Module.Api.Farm.Models;

namespace Module.Api.Farm.GraphQL.ResultTypes
{
    public class FarmPageListType : ObjectType<FarmPageListModel>
    {
        protected override void Configure(IObjectTypeDescriptor<FarmPageListModel> descriptor)
        {
            descriptor.Field(x => x.Collections).Type<ListType<FarmResultType>>();
            descriptor.Field(x => x.TotalPage).Type<IntType>();
            descriptor.Field(x => x.TotalResult).Type<IntType>();
            descriptor.Field(x => x.Filter).Type<BaseFilterResultType>();
        }
    }
}
