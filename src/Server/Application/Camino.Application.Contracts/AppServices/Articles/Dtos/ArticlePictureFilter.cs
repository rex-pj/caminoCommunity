using System;

namespace Camino.Application.Contracts.AppServices.Articles.Dtos
{
    public class ArticlePictureFilter : BaseFilter
    {
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public string MimeType { get; set; }
    }
}
