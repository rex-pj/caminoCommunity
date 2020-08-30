using Camino.Service.Data.Content;

namespace Camino.Service.Business.Media.Contracts
{
    public interface IPictureBusiness
    {
        PictureProjection Find(long id);
    }
}