using Camino.Framework.Models;
using Camino.Shared.General;
using Module.Api.Product.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Resolvers.Contracts
{
    public interface IProductAttributeResolver
    {
        Task<IEnumerable<SelectOption>> GetProductAttributesAsync(AttributeSelectFilterModel criterias);
        IList<SelectOption> GetProductAttributeControlTypes(AttributeControlTypeSelectFilterModel filter);
    }
}
