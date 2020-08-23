using System;

namespace Camino.Service.Data.Filters
{
    public class ArticleFilter : BaseFilter
    {
        public string Content { get; set; }
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public long? CategoryId { get; set; }
    }
}
