using Camino.Application.Contracts;
using Module.Product.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Product.Api.GraphQL.Resolvers.Contracts
{
    public interface IProductAttributeResolver
    {
        Task<IEnumerable<SelectOption>> GetProductAttributesAsync(AttributeSelectFilterModel criterias);
        IList<SelectOption> GetProductAttributeControlTypes(AttributeControlTypeSelectFilterModel filter);
    }
}
