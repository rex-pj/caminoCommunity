using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Camino.Application.Contracts.AppServices.Farms.Dtos
{
    public class FarmPicturesModifyRequest
    {
        public FarmPicturesModifyRequest()
        {
            Pictures = new List<PictureRequest>();
        }

        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        public long FarmId { get; set; }

        public IEnumerable<PictureRequest> Pictures { get; set; }
    }
}
