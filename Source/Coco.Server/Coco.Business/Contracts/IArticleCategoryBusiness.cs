using Coco.Entities.Dtos.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IArticleCategoryBusiness
    {
        ArticleCategoryDto Find(int id);
        Task<List<ArticleCategoryDto>> GetAsync();
        public long Add(ArticleCategoryDto category);
        ArticleCategoryDto Update(ArticleCategoryDto category);
    }
}
