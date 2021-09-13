using Camino.Core.Contracts.Services.Articles;
using Camino.Shared.Results.Articles;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using Camino.Shared.General;
using Module.Api.Article.Models;
using Camino.Shared.Requests.Filters;
using Camino.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace Module.Api.Article.GraphQL.Resolvers
{
    public class ArticleCategoryResolver : IArticleCategoryResolver
    {
        private readonly IArticleCategoryService _articleCategoryService;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public ArticleCategoryResolver(IArticleCategoryService articleCategoryService, IOptions<PagerOptions> pagerOptions)
        {
            _articleCategoryService = articleCategoryService;
            _pagerOptions = pagerOptions.Value;
        }

        public IEnumerable<SelectOption> GetArticleCategories(ArticleCategorySelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ArticleCategorySelectFilterModel();
            }

            var filter = new BaseFilter
            {
                Keyword = criterias.Query,
                PageSize = _pagerOptions.PageSize,
                Page = _defaultPageSelection
            };
            IList<ArticleCategoryResult> categories;
            if (criterias.IsParentOnly.HasValue && criterias.IsParentOnly.GetValueOrDefault())
            {
                categories = _articleCategoryService.SearchParents(new IdRequestFilter<int?>
                {
                    Id = criterias.CurrentId
                }, filter);
            }
            else
            {
                categories = _articleCategoryService.Search(new IdRequestFilter<int?>
                {
                    Id = criterias.CurrentId
                }, filter);
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
