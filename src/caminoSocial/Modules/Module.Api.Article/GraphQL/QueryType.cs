using Camino.Core.Modular.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using HotChocolate.Types;
using Module.Api.Article.GraphQL.InputTypes;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using Module.Api.Article.GraphQL.ResultTypes;

namespace Module.Api.Article.GraphQL
{
    public class QueryType : BaseQueryType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IArticleResolver>(x => x.GetUserArticlesAsync(default))
                .Type<ArticlePageListType>()
                .Argument("criterias", a => a.Type<ArticleFilterInputType>());

            descriptor.Field<IArticleResolver>(x => x.GetArticlesAsync(default))
                .Type<ArticlePageListType>()
                .Argument("criterias", a => a.Type<ArticleFilterInputType>());

            descriptor.Field<IArticleResolver>(x => x.GetArticleAsync(default))
                .Type<ArticleResultType>()
                .Argument("criterias", a => a.Type<ArticleFilterInputType>());

            descriptor.Field<IArticleResolver>(x => x.GetRelevantArticlesAsync(default))
                .Type<ListType<ArticleResultType>>()
                .Argument("criterias", a => a.Type<ArticleFilterInputType>());
        }
    }
}
