using Camino.Application.Contracts;
using Module.Article.Api.Models;
using System.Collections.Generic;

namespace Module.Article.Api.GraphQL.Resolvers.Contracts
{
    public interface IArticleCategoryResolver
    {
        IEnumerable<SelectOption> GetArticleCategories(ArticleCategorySelectFilterModel criterias);
    }
}
