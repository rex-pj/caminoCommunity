using System;

namespace Camino.Business.Dtos.General
{
    public class ArticleFilterDto : BaseFilterDto
    {
        public string Content { get; set; }
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
    }
}
