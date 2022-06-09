using Camino.Framework.Models;
using Camino.Shared.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.ProductManagement.Models
{
    public class UpdateProductModel
    {
        public UpdateProductModel()
        {
            ProductCategories = new List<ProductCategoryRelationModel>();
            ProductFarms = new List<ProductFarmModel>();
            ProductAttributes = new List<ProductAttributeRelationModel>();
        }

        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }

        public IEnumerable<ProductCategoryRelationModel> ProductCategories { get; set; }
        public IEnumerable<int> ProductCategoryIds { get; set; }
        public IEnumerable<ProductFarmModel> ProductFarms { get; set; }
        public IEnumerable<long> ProductFarmIds { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<PictureRequestModel> Pictures { get; set; }
        public IFormFile File { get; set; }
        public ProductStatuses StatusId { get; set; }
        public IEnumerable<ProductAttributeRelationModel> ProductAttributes { get; set; }
    }
}
