using Camino.Application.Contracts;
using Module.Api.Article.Models;
using System.Collections.Generic;

namespace Module.Api.Article.GraphQL.Resolvers.Contracts
{
    public interface IArticleCategoryResolver
    {
        IEnumerable<SelectOption> GetArticleCategories(ArticleCategorySelectFilterModel criterias);
    }
}
