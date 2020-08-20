using Camino.Service.Data.Content;
using Camino.Service.Data.Filters;
using System.Threading.Tasks;
using Camino.Service.Data.Page;

namespace Camino.Service.Business.Articles.Contracts
{
    public interface IArticleBusiness
    {
        int Add(ArticleResult category);
        ArticleResult Find(int id);
        ArticleResult FindByName(string name);
        ArticleResult Update(ArticleResult article);
        Task<PageList<ArticleResult>> GetAsync(ArticleFilter filter);
    }
}