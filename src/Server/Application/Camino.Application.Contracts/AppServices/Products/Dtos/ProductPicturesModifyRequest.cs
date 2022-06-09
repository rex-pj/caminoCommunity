using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Camino.Application.Contracts.AppServices.Products.Dtos
{
    public class ProductPicturesModifyRequest
    {
        public ProductPicturesModifyRequest()
        {
            Pictures = new List<PictureRequest>();
        }

        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }

        public long ProductId { get; set; }

        public IEnumerable<PictureRequest> Pictures { get; set; }
    }
}
