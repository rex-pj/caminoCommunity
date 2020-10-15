using Camino.Core.Models;
using Camino.Framework.Models;
using Camino.Service.Business.Articles.Contracts;
using Camino.Service.Projections.Article;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Module.Api.Article.GraphQL.Resolvers
{
    public class ArticleCategoryResolver : IArticleCategoryResolver
    {
        private readonly IArticleCategoryBusiness _articleCategoryBusiness;
        public ArticleCategoryResolver(IArticleCategoryBusiness articleCategoryBusiness)
        {
            _articleCategoryBusiness = articleCategoryBusiness;
        }

        public IEnumerable<ISelectOption> GetArticleCategories(SelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new SelectFilterModel();
            }

            IList<ArticleCategoryProjection> categories;
            if (criterias.IsParentOnly)
            {
                categories = _articleCategoryBusiness.SearchParents(criterias.Query, criterias.CurrentId);
            }
            else
            {
                categories = _articleCategoryBusiness.Search(criterias.Query, criterias.CurrentId);
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
