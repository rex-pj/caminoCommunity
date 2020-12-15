using Camino.Framework.Models;
using System;

namespace Module.Api.Article.Models
{
    public class ArticleModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public PictureRequestModel Thumbnail { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public string CreatedByIdentityId { get; set; }
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }
    }
}
