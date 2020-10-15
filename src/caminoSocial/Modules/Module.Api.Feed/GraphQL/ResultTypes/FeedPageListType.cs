using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Module.Api.Feed.Models;

namespace Module.Api.Feed.GraphQL.ResultTypes
{
    public class FeedPageListType : ObjectType<FeedPageListModel>
    {
        protected override void Configure(IObjectTypeDescriptor<FeedPageListModel> descriptor)
        {
            descriptor.Field(x => x.Collections).Type<ListType<FeedResultType>>();
            descriptor.Field(x => x.TotalPage).Type<IntType>();
            descriptor.Field(x => x.TotalResult).Type<IntType>();
            descriptor.Field(x => x.Filter).Type<BaseFilterResultType>();
        }
    }
}
