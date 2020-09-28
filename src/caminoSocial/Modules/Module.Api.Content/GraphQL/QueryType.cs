using Camino.Core.Modular.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using HotChocolate.Types;
using Module.Api.Content.GraphQL.InputTypes;
using Module.Api.Content.GraphQL.Resolvers.Contracts;
using Module.Api.Content.GraphQL.ResultTypes;

namespace Module.Api.Content.GraphQL
{
    public class QueryType : BaseQueryType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IArticleResolver>(x => x.GetUserArticlesAsync(default))
                .Type<ArticlePageListType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<ArticleFilterInputType>());
        }
    }
}
