using Camino.Service.Projections.Content;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using System.Threading.Tasks;

namespace Camino.Service.Business.Articles.Contracts
{
    public interface IArticlePictureBusiness
    {
        Task<BasePageList<ArticlePictureProjection>> GetAsync(ArticlePictureFilter filter);
    }
}
