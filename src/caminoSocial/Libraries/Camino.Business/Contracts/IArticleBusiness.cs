using Camino.Business.Dtos.Content;
using System.Collections.Generic;

namespace Camino.Business.Contracts
{
    public interface IArticleBusiness
    {
        int Add(ArticleDto category);
        ArticleDto Find(int id);
        ArticleDto FindByName(string name);
        List<ArticleDto> GetFull();
        ArticleDto Update(ArticleDto article);
    }
}