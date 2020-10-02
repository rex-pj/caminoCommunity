using Camino.Service.Projections.Content;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using System.Threading.Tasks;

namespace Camino.Service.Business.Products.Contracts
{
    public interface IProductPictureBusiness
    {
        Task<BasePageList<ProductPictureProjection>> GetAsync(ProductPictureFilter filter);
    }
}
