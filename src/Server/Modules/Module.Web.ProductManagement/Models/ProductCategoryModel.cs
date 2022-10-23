using Camino.Infrastructure.Identity.Models;
using Camino.Shared.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.ProductManagement.Models
{
    public class ProductCategoryModel : BaseIdentityModel
    {
        public ProductCategoryModel()
        {
            SelectCategories = new List<SelectListItem>();
        }

        public long Id { get; set; }

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
        public long? ParentId { get; set; }
        public string ParentCategoryName { get; set; }
        public ProductCategoryStatuses StatusId { get; set; }
        public IEnumerable<SelectListItem> SelectCategories { get; set; }
    }
}
