using Module.Article.Api.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using Module.Article.Api.Models;
using Microsoft.Extensions.Options;
using Camino.Application.Contracts.AppServices.Articles;
using Camino.Shared.Configuration.Options;
using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Articles.Dtos;

namespace Module.Article.Api.GraphQL.Resolvers
{
    public class ArticleCategoryResolver : IArticleCategoryResolver
    {
        private readonly IArticleCategoryAppService _articleCategoryAppService;
        private readonly PagerOptions _pagerOptions;
        private const int _defaultPageSelection = 1;

        public ArticleCategoryResolver(IArticleCategoryAppService articleCategoryAppService, IOptions<PagerOptions> pagerOptions)
        {
            _articleCategoryAppService = articleCategoryAppService;
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
                categories = _articleCategoryAppService.SearchParents(new IdRequestFilter<int?>
                {
                    Id = criterias.CurrentId
                }, filter);
            }
            else
            {
                categories = _articleCategoryAppService.Search(new IdRequestFilter<int?>
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
