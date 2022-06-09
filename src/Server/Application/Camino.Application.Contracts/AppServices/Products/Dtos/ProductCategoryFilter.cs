using System;

namespace Camino.Application.Contracts.AppServices.Products.Dtos
{
    public class ProductCategoryFilter : BaseFilter
    {
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int? StatusId { get; set; }
    }
}
