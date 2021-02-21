using Camino.Core.Contracts.Services.Products;
using Camino.Shared.Results.Products;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Shared.General;
using Module.Api.Product.Models;

namespace Module.Api.Product.GraphQL.Resolvers
{
    public class ProductCategoryResolver : IProductCategoryResolver
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoryResolver(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public async Task<IEnumerable<SelectOption>> GetProductCategoriesAsync(ProductCategorySelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ProductCategorySelectFilterModel();
            }

            IList<ProductCategoryResult> categories;
            if (criterias.IsParentOnly)
            {
                categories = await _productCategoryService.SearchParentsAsync(criterias.CurrentIds, criterias.Query);
            }
            else
            {
                categories = await _productCategoryService.SearchAsync(criterias.CurrentIds, criterias.Query);
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
