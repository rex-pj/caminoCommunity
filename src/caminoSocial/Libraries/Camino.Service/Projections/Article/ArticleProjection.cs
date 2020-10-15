using System;

namespace Camino.Service.Projections.Article
{
    public class ArticleProjection
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }

        // Thumbnail Picture
        public long ThumbnailId { get; set; }
        public string Thumbnail { get; set; }
        public string ThumbnailFileType { get; set; }
        public string ThumbnailFileName { get; set; }
    }
}
