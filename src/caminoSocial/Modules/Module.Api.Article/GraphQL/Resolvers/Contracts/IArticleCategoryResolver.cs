using Camino.Core.Models;
using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Api.Article.GraphQL.Resolvers.Contracts
{
    public interface IArticleCategoryResolver
    {
        IEnumerable<SelectOption> GetArticleCategories(SelectFilterModel criterias);
    }
}
