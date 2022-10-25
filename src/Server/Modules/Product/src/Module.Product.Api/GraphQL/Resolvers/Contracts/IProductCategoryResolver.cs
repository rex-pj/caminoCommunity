using Camino.Application.Contracts;
using Module.Product.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Product.Api.GraphQL.Resolvers.Contracts
{
    public interface IProductCategoryResolver
    {
        Task<IEnumerable<SelectOption>> GetProductCategoriesAsync(ProductCategorySelectFilterModel criterias);
    }
}
