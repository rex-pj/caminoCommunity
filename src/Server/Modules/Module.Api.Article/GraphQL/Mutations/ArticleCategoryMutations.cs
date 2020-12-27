using Camino.Core.Models;
using Camino.Framework.GraphQL.Mutations;
using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;

namespace Module.Api.Article.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class ArticleCategoryMutations : BaseMutations
    {
        public IEnumerable<SelectOption> GetArticleCategories([Service] IArticleCategoryResolver articleCategoryResolver, SelectFilterModel criterias)
        {
            return articleCategoryResolver.GetArticleCategories(criterias);
        }
    }
}
