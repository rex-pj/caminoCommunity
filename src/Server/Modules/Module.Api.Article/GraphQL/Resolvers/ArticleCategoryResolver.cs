﻿using Camino.Core.Contracts.Services.Articles;
using Camino.Shared.Results.Articles;
using Module.Api.Article.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using System.Linq;
using Camino.Shared.General;
using Module.Api.Article.Models;
using Camino.Shared.Requests.Filters;

namespace Module.Api.Article.GraphQL.Resolvers
{
    public class ArticleCategoryResolver : IArticleCategoryResolver
    {
        private readonly IArticleCategoryService _articleCategoryService;
        public ArticleCategoryResolver(IArticleCategoryService articleCategoryService)
        {
            _articleCategoryService = articleCategoryService;
        }

        public IEnumerable<SelectOption> GetArticleCategories(ArticleCategorySelectFilterModel criterias)
        {
            if (criterias == null)
            {
                criterias = new ArticleCategorySelectFilterModel();
            }

            IList<ArticleCategoryResult> categories;
            if (criterias.IsParentOnly)
            {
                categories = _articleCategoryService.SearchParents(new IdRequestFilter<int?>
                {
                    Id = criterias.CurrentId
                }, criterias.Query);
            }
            else
            {
                categories = _articleCategoryService.Search(new IdRequestFilter<int?>
                {
                    Id = criterias.CurrentId
                }, criterias.Query);
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
