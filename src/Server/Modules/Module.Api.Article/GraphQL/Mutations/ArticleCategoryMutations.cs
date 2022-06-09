using Camino.Application.Contracts;
using Camino.Framework.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using Module.Api.Article.Models;
using System.Collections.Generic;

namespace Module.Api.Article.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class ArticleCategoryMutations : BaseMutations
    {
        public IEnumerable<SelectOption> GetArticleCategories([Service] IArticleCategoryResolver articleCategoryResolver, ArticleCategorySelectFilterModel criterias)
        {
            return articleCategoryResolver.GetArticleCategories(criterias);
        }
    }
}
