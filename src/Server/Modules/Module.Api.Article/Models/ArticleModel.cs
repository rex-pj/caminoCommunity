using Camino.Infrastructure.AspNetCore.Models;
using System;

namespace Module.Api.Article.Models
{
    public class ArticleModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public PictureResultModel Picture { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string CreatedBy { get; set; }
        public long CreatedByPhotoId { get; set; }
        public string CreatedByIdentityId { get; set; }
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }
    }
}
