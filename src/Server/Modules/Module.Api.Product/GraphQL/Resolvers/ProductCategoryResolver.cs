using Camino.Core.Models;
using Camino.Framework.Models;
using Camino.Service.Business.Products.Contracts;
using Camino.Service.Projections.Product;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Resolvers
{
    public class ProductCategoryResolver : IProductCategoryResolver
    {
        private readonly IProductCategoryBusiness _productCategoryBusiness;
        public ProductCategoryResolver(IProductCategoryBusiness productCategoryBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
        }

        public async Task<IEnumerable<SelectOption>> GetProductCategoriesAsync(SelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new SelectFilterModel();
            }

            IList<ProductCategoryProjection> categories;
            if (criterias.IsParentOnly)
            {
                categories = await _productCategoryBusiness.SearchParentsAsync(criterias.CurrentIds, criterias.Query);
            }
            else
            {
                categories = await _productCategoryBusiness.SearchAsync(criterias.CurrentIds, criterias.Query);
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
