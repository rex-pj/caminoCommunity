using Camino.Service.Projections.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Camino.DAL.Entities;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Article;

namespace Camino.Service.Business.Articles.Contracts
{
    public interface IArticleCategoryBusiness
    {
        ArticleCategoryProjection Find(long id);
        Task<BasePageList<ArticleCategoryProjection>> GetAsync(ArticleCategoryFilter filter);
        IList<ArticleCategoryProjection> Search(string search = "", long? currentId = null, int page = 1, int pageSize = 10);
        IList<ArticleCategoryProjection> SearchParents(string search = "", long? currentId = null, int page = 1, int pageSize = 10);
        List<ArticleCategoryProjection> Get(Expression<Func<ArticleCategory, bool>> filter);
        public int Create(ArticleCategoryProjection category);
        ArticleCategoryProjection Update(ArticleCategoryProjection category);
        ArticleCategoryProjection FindByName(string name);
    }
}
