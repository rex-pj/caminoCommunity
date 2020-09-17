using Camino.Core.Models;
using Module.Api.Content.Models;
using System.Collections.Generic;

namespace Module.Api.Content.GraphQL.Resolvers.Contracts
{
    public interface IArticleCategoryResolver
    {
        IEnumerable<ISelectOption> GetCategories(SelectFilterModel criterias);
    }
}
