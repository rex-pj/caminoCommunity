using Camino.Core.Models;
using Camino.Framework.Models;
using Camino.Service.Business.Products.Contracts;
using Camino.Service.Projections.Content;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Module.Api.Product.GraphQL.Resolvers
{
    public class ProductCategoryResolver : IProductCategoryResolver
    {
        private readonly IProductCategoryBusiness _productCategoryBusiness;
        public ProductCategoryResolver(IProductCategoryBusiness productCategoryBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
        }

        public IEnumerable<ISelectOption> GetProductCategories(SelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new SelectFilterModel();
            }

            IList<ProductCategoryProjection> categories;
            if (criterias.IsParentOnly)
            {
                categories = _productCategoryBusiness.SearchParents(criterias.Query, criterias.CurrentId);
            }
            else
            {
                categories = _productCategoryBusiness.Search(criterias.Query, criterias.CurrentId);
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
