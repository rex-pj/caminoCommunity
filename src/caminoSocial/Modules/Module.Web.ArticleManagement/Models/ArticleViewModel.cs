using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Module.Web.ArticleManagement.Models
{
    public class ArticleViewModel
    {
        public ArticleViewModel()
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
        public DateTime UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }

        public IEnumerable<SelectListItem> SelectCategories { get; set; }
    }
}
