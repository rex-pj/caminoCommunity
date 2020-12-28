using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.ProductManagement.Models
{
    public class ProductModel : BaseModel
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
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
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public IFormFile File { get; set; }
    }
}
