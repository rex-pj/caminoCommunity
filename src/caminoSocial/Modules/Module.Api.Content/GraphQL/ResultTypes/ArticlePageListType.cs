using Camino.Framework.Models;
using HotChocolate.Types;
using Module.Api.Content.Models;

namespace Module.Api.Content.GraphQL.ResultTypes
{
    public class ArticlePageListType : ObjectType<PageListModel<ArticleModel>>
    {
        protected override void Configure(IObjectTypeDescriptor<PageListModel<ArticleModel>> descriptor)
        {
            descriptor.Field(x => x.Collections).Type<ListType<ArticleResultType>>();
        }
    }
}
