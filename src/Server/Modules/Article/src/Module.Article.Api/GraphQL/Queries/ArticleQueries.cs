﻿using Camino.Infrastructure.GraphQL.Queries;
using HotChocolate;
using HotChocolate.Types;
using Module.Article.Api.GraphQL.Resolvers.Contracts;
using Module.Article.Api.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Article.Api.GraphQL.Queries
{
    [ExtendObjectType("Query")]
    public class ArticleQueries : BaseQueries
    {
        public async Task<ArticlePageListModel> GetUserArticlesAsync(ClaimsPrincipal claimsPrincipal, [Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetUserArticlesAsync(claimsPrincipal, criterias);
        }

        public async Task<ArticlePageListModel> GetArticlesAsync([Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetArticlesAsync(criterias);
        }

        public async Task<ArticleModel> GetArticleAsync([Service] IArticleResolver articleResolver, ArticleIdFilterModel criterias)
        {
            return await articleResolver.GetArticleAsync(criterias);
        }

        public async Task<IList<ArticleModel>> GetRelevantArticlesAsync([Service] IArticleResolver articleResolver, ArticleFilterModel criterias)
        {
            return await articleResolver.GetRelevantArticlesAsync(criterias);
        }
    }
}
