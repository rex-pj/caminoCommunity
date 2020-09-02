using Camino.Service.Data.Content;
using Camino.Service.Data.Filters;
using Camino.Service.Data.PageList;
using System.Threading.Tasks;

namespace Camino.Service.Business.Articles.Contracts
{
    public interface IArticlePictureBusiness
    {
        Task<BasePageList<ArticlePictureProjection>> GetAsync(ArticlePictureFilter filter);
    }
}
