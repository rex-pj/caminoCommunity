using System;

namespace Camino.Business.Dtos.General
{
    public class ArticleCategoryFilterDto : BaseFilterDto
    {
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
    }
}
