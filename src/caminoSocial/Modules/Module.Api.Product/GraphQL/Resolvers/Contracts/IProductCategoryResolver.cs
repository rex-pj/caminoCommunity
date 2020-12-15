using Camino.Core.Models;
using Camino.Framework.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Resolvers.Contracts
{
    public interface IProductCategoryResolver
    {
        Task<IEnumerable<SelectOption>> GetProductCategoriesAsync(SelectFilterModel criterias);
    }
}
