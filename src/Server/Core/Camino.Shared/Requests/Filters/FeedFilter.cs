using System;

namespace Camino.Shared.Requests.Filters
{
    public class FeedFilter : BaseFilter
    {
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public long? FeedTypeId { get; set; }
        public bool CanGetInactived { get; set; }
        public bool CanGetDeleted { get; set; }
    }
}
