using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Camino.Application.Contracts.AppServices.Farms.Dtos
{
    public class FarmResult
    {
        public FarmResult()
        {
            Pictures = new List<PictureResult>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public long FarmTypeId { get; set; }
        public string FarmTypeName { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public string Address { get; set; }
        public int StatusId { get; set; }
        public IEnumerable<PictureResult> Pictures { get; set; }
    }
}
