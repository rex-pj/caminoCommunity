using Camino.Application.Contracts;
using Camino.Infrastructure.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Types;
using Module.Article.Api.GraphQL.Resolvers.Contracts;
using Module.Article.Api.Models;
using System.Collections.Generic;

namespace Module.Article.Api.GraphQL.Mutations
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
