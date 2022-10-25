using Camino.Infrastructure.Identity.Models;
using Camino.Shared.Enums;
using System;

namespace Module.Article.WebAdmin.Models
{
    public class ArticleModel : BaseIdentityModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }
        public long PictureId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }
        public ArticleStatuses StatusId { get; set; }
    }
}
