using Module.Product.Api.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Module.Product.Api.Models;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Shared.Configuration.Options;
using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Products.Dtos;

namespace Module.Product.Api.GraphQL.Resolvers
{
    public class ProductCategoryResolver : IProductCategoryResolver
    {
        private readonly IProductCategoryAppService _productCategoryAppService;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public ProductCategoryResolver(IProductCategoryAppService productCategoryAppService, IOptions<PagerOptions> pagerOptions)
        {
            _productCategoryAppService = productCategoryAppService;
            _pagerOptions = pagerOptions.Value;
        }

        public async Task<IEnumerable<SelectOption>> GetProductCategoriesAsync(ProductCategorySelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ProductCategorySelectFilterModel();
            }

            IList<ProductCategoryResult> categories;
            var filter = new BaseFilter
            {
                Keyword = criterias.Query,
                PageSize = _pagerOptions.PageSize,
                Page = _defaultPageSelection
            };
            if (criterias.IsParentOnly.HasValue && criterias.IsParentOnly.GetValueOrDefault())
            {
                categories = await _productCategoryAppService.SearchParentsAsync(filter, criterias.CurrentIds);
            }
            else
            {
                categories = await _productCategoryAppService.SearchAsync(filter, criterias.CurrentIds);
            }

            if (categories == null || !categories.Any())
            {
                return new List<SelectOption>();
            }

            var categorySeletions = categories
                .Select(x => new SelectOption
                {
                    Id = x.Id.ToString(),
                    Text = x.ParentId.HasValue ? $"-- {x.Name}" : x.Name
                });

            return categorySeletions;
        }
    }
}
