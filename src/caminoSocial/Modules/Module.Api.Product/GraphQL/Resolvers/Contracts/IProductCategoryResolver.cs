using Camino.Core.Models;
using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Api.Product.GraphQL.Resolvers.Contracts
{
    public interface IProductCategoryResolver
    {
        IEnumerable<ISelectOption> GetProductCategories(SelectFilterModel criterias);
    }
}
