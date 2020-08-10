using Camino.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.ArticleManagement.Models
{
    public class ArticleCategoryModel : BaseModel
    {
        public ArticleCategoryModel()
        {
            SelectCategories = new List<SelectListItem>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public int? ParentId { get; set; }
        public string ParentCategoryName { get; set; }
        public IEnumerable<SelectListItem> SelectCategories { get; set; }
    }
}
