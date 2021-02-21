using Camino.Shared.General;
using Module.Api.Product.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Resolvers.Contracts
{
    public interface IProductCategoryResolver
    {
        Task<IEnumerable<SelectOption>> GetProductCategoriesAsync(ProductCategorySelectFilterModel criterias);
    }
}
