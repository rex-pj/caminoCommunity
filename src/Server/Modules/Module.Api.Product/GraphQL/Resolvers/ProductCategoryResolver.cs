using Camino.Core.Contracts.Services.Products;
using Camino.Shared.Results.Products;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.General;
using Module.Api.Product.Models;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace Module.Api.Product.GraphQL.Resolvers
{
    public class ProductCategoryResolver : IProductCategoryResolver
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public ProductCategoryResolver(IProductCategoryService productCategoryService, IOptions<PagerOptions> pagerOptions)
        {
            _productCategoryService = productCategoryService;
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
            if (criterias.IsParentOnly)
            {
                categories = await _productCategoryService.SearchParentsAsync(filter, criterias.CurrentIds);
            }
            else
            {
                categories = await _productCategoryService.SearchAsync(filter, criterias.CurrentIds);
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
