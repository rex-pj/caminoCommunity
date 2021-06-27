using System;

namespace Camino.Shared.Requests.Filters
{
    public class ArticleCategoryFilter : BaseFilter
    {
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public int? StatusId { get; set; }
        public bool CanGetDeleted { get; set; }
        public bool CanGetInactived { get; set; }
    }
}
