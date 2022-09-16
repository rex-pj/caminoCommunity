namespace Camino.Application.Contracts.AppServices.Products.Dtos
{
    public class ProductFilter : BaseFilter
    {
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public long? CategoryId { get; set; }
        public long? FarmId { get; set; }
        public int? StatusId { get; set; }
        public bool CanGetDeleted { get; set; }
        public bool CanGetInactived { get; set; }
    }
}
