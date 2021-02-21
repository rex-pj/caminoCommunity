using Camino.Core.Contracts.Services.Products;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using Module.Api.Product.Models;
using Camino.Framework.Models;

namespace Module.Api.Product.GraphQL.Resolvers
{
    public class ProductAttributeResolver : IProductAttributeResolver
    {
        private readonly IProductAttributeService _productAttributeService;
        public ProductAttributeResolver(IProductAttributeService productAttributeService)
        {
            _productAttributeService = productAttributeService;
        }

        public async Task<IEnumerable<SelectOption>> GetProductAttributesAsync(BaseSelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new BaseSelectFilterModel();
            }

            var attributes = await _productAttributeService.SearchAsync(criterias.Query);
            if (attributes == null || !attributes.Any())
            {
                return new List<SelectOption>();
            }

            var attributeSeletions = attributes
                .Select(x => new SelectOption
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return attributeSeletions;
        }

        public IList<SelectOption> GetProductAttributeControlTypes(ProductAttributeControlTypeSelectFilterModel filter)
        {
            var serviceFilter = new ProductAttributeControlTypeFilter
            {
                ControlTypeId = filter.CurrentId,
                Search = filter.Query
            };
            return _productAttributeService.GetAttributeControlTypes(serviceFilter);
        }
    }
}
