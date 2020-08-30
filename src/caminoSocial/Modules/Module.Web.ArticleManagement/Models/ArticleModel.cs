using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.ArticleManagement.Models
{
    public class ArticleModel : BaseModel
    {
        public ArticleModel()
        {

        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }
        public long ThumbnailId { get; set; }
        public string Thumbnail { get; set; }
        public string ThumbnailFileType { get; set; }
        public string ThumbnailFileName { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }
        public IFormFile File { get; set; }

        public IEnumerable<SelectListItem> SelectCategories { get; set; }
    }
}
