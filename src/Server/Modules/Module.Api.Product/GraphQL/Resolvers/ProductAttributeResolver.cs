using Module.Api.Product.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Module.Api.Product.Models;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Shared.Configuration.Options;
using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Products.Dtos;

namespace Module.Api.Product.GraphQL.Resolvers
{
    public class ProductAttributeResolver : IProductAttributeResolver
    {
        private readonly IProductAttributeAppService _productAttributeAppService;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public ProductAttributeResolver(IProductAttributeAppService productAttributeAppService, IOptions<PagerOptions> pagerOptions)
        {
            _productAttributeAppService = productAttributeAppService;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<IEnumerable<SelectOption>> GetProductAttributesAsync(AttributeSelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new AttributeSelectFilterModel();
            }

            var attributes = await _productAttributeAppService.SearchAsync(new ProductAttributeFilter
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
            return _productAttributeAppService.GetAttributeControlTypes(serviceFilter);
        }
    }
}
