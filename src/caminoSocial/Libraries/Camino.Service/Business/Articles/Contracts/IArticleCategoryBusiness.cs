using Camino.Service.Data.Content;
using Camino.Service.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Camino.DAL.Entities;
using Camino.Service.Data.PageList;

namespace Camino.Service.Business.Articles.Contracts
{
    public interface IArticleCategoryBusiness
    {
        ArticleCategoryProjection Find(int id);
        Task<BasePageList<ArticleCategoryProjection>> GetAsync(ArticleCategoryFilter filter);
        List<ArticleCategoryProjection> Get();
        List<ArticleCategoryProjection> Get(Expression<Func<ArticleCategory, bool>> filter);
        public int Add(ArticleCategoryProjection category);
        ArticleCategoryProjection Update(ArticleCategoryProjection category);
        ArticleCategoryProjection FindByName(string name);
    }
}
