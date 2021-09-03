using Camino.Shared.Results.Media;
using System;

namespace Camino.Shared.Results.Articles
{
    public class ArticleResult
    {
        public ArticleResult()
        {
            Picture = new PictureResult();
        }

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
        public PictureResult Picture { get; set; }
        public int StatusId { get; set; }
    }
}
