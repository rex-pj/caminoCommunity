using Camino.Service.Projections.Filters;
using Camino.Service.Projections.PageList;
using Camino.Service.Projections.Product;
using System.Threading.Tasks;

namespace Camino.Service.Business.Products.Contracts
{
    public interface IProductPictureBusiness
    {
        Task<BasePageList<ProductPictureProjection>> GetAsync(ProductPictureFilter filter);
    }
}
