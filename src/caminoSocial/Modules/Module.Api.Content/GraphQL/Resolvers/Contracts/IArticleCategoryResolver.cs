using Camino.Core.Models;
using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Api.Content.GraphQL.Resolvers.Contracts
{
    public interface IArticleCategoryResolver
    {
        IEnumerable<ISelectOption> GetArticleCategories(SelectFilterModel criterias);
    }
}
