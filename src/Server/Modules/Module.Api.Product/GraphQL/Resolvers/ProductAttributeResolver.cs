using Camino.Core.Contracts.Services.Products;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.General;
using Camino.Shared.Requests.Filters;
using Module.Api.Product.Models;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace Module.Api.Product.GraphQL.Resolvers
{
    public class ProductAttributeResolver : IProductAttributeResolver
    {
        private readonly IProductAttributeService _productAttributeService;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public ProductAttributeResolver(IProductAttributeService productAttributeService, IOptions<PagerOptions> pagerOptions)
        {
            _productAttributeService = productAttributeService;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<IEnumerable<SelectOption>> GetProductAttributesAsync(AttributeSelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new AttributeSelectFilterModel();
            }

            var attributes = await _productAttributeService.SearchAsync(new ProductAttributeFilter
            {
                ExcludedIds = criterias.ExcludedIds,
                Keyword = criterias.Query,
                Page = _defaultPageSelection,
                PageSize = _pagerOptions.PageSize
            });
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

        public IList<SelectOption> GetProductAttributeControlTypes(AttributeControlTypeSelectFilterModel filter)
        {
            var serviceFilter = new ProductAttributeControlTypeFilter
            {
                ControlTypeId = filter.CurrentId.GetValueOrDefault(),
                Keyword = filter.Query
            };
            return _productAttributeService.GetAttributeControlTypes(serviceFilter);
        }
    }
}
