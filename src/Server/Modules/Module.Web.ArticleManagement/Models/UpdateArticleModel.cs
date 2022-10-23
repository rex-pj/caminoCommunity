using Camino.Infrastructure.Identity.Models;
using Camino.Shared.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.ArticleManagement.Models
{
    public class UpdateArticleModel : BaseIdentityModel
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }
        public long PictureId { get; set; }
        public string Picture { get; set; }
        public string PictureFileType { get; set; }
        public string PictureFileName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }
        public ArticleStatuses StatusId { get; set; }
        public IFormFile File { get; set; }
    }
}
