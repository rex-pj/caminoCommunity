using Camino.Business.Dtos.Content;
using Camino.Business.Dtos.General;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camino.Business.Contracts
{
    public interface IArticleBusiness
    {
        int Add(ArticleDto category);
        ArticleDto Find(int id);
        ArticleDto FindByName(string name);
        ArticleDto Update(ArticleDto article);
        Task<PageListDto<ArticleDto>> GetAsync(ArticleFilterDto filter);
    }
}