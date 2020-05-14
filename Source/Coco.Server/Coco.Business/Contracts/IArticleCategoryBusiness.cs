using Coco.Entities.Domain.Content;
using Coco.Entities.Dtos.Content;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Coco.Business.Contracts
{
    public interface IArticleCategoryBusiness
    {
        ArticleCategoryDto Find(int id);
        List<ArticleCategoryDto> GetFull();
        List<ArticleCategoryDto> Get(Expression<Func<ArticleCategory, bool>> filter);
        public long Add(ArticleCategoryDto category);
        ArticleCategoryDto Update(ArticleCategoryDto category);
        ArticleCategoryDto FindByName(string name);
    }
}
