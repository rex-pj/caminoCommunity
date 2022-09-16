namespace Camino.Application.Contracts.AppServices.Farms.Dtos
{
    public class FarmFilter : BaseFilter
    {
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public long? FarmTypeId { get; set; }
        public long? ExclusiveUserId { get; set; }
        public bool CanGetDeleted { get; set; }
        public int? StatusId { get; set; }
        public bool CanGetInactived { get; set; }
    }
}
