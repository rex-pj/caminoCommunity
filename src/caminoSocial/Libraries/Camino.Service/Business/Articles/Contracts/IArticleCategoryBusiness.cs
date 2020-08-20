using Camino.Service.Data.Content;
using Camino.Service.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Camino.DAL.Entities;
using Camino.Service.Data.Page;

namespace Camino.Service.Business.Articles.Contracts
{
    public interface IArticleCategoryBusiness
    {
        ArticleCategoryResult Find(int id);
        Task<PageList<ArticleCategoryResult>> GetAsync(ArticleCategoryFilter filter);
        List<ArticleCategoryResult> Get();
        List<ArticleCategoryResult> Get(Expression<Func<ArticleCategory, bool>> filter);
        public int Add(ArticleCategoryResult category);
        ArticleCategoryResult Update(ArticleCategoryResult category);
        ArticleCategoryResult FindByName(string name);
    }
}
