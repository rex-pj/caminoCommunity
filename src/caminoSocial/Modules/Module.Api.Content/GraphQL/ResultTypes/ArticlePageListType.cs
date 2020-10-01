using Camino.Framework.GraphQL.ResultTypes;
using HotChocolate.Types;
using Module.Api.Content.Models;

namespace Module.Api.Content.GraphQL.ResultTypes
{
    public class ArticlePageListType : ObjectType<ArticlePageListModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticlePageListModel> descriptor)
        {
            descriptor.Field(x => x.Collections).Type<ListType<ArticleResultType>>();
            descriptor.Field(x => x.TotalPage).Type<IntType>();
            descriptor.Field(x => x.TotalResult).Type<IntType>();
            descriptor.Field(x => x.Filter).Type<BaseFilterResultType>();
        }
    }
}
