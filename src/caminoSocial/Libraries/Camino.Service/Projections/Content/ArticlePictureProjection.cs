using System;

namespace Camino.Service.Projections.Content
{
    public class ArticlePictureProjection
    {
        public long ArticleId { get; set; }
        public string ArticleName { get; set; }
        public int ArticlePictureType { get; set; }
        public long PictureId { get; set; }
        public string PictureName { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset PictureCreatedDate { get; set; }
        public long PictureCreatedById { get; set; }
        public string PictureCreatedBy { get; set; }
    }
}
