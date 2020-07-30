using Camino.Business.Dtos.Content;
using Camino.Data.Entities.Content;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Camino.Business.Contracts
{
    public interface IArticleCategoryBusiness
    {
        ArticleCategoryDto Find(int id);
        List<ArticleCategoryDto> GetFull();
        List<ArticleCategoryDto> Get();
        List<ArticleCategoryDto> Get(Expression<Func<ArticleCategory, bool>> filter);
        public int Add(ArticleCategoryDto category);
        ArticleCategoryDto Update(ArticleCategoryDto category);
        ArticleCategoryDto FindByName(string name);
    }
}
