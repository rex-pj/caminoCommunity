using System;

namespace Camino.Shared.Requests.Filters
{
    public class ArticleFilter : BaseFilter
    {
        public string Content { get; set; }
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public long? CategoryId { get; set; }
        public bool CanGetDeleted { get; set; }
    }
}
