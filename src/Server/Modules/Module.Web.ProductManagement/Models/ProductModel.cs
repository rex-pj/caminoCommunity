using Camino.Framework.Models;
using Camino.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Module.Web.ProductManagement.Models
{
    public class ProductModel : BaseModel
    {
        public ProductModel()
        {
            ProductCategories = new List<ProductCategoryRelationModel>();
            ProductFarms = new List<ProductFarmModel>();
            ProductAttributes = new List<ProductAttributeRelationModel>();
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public IEnumerable<ProductCategoryRelationModel> ProductCategories { get; set; }
        public IEnumerable<ProductFarmModel> ProductFarms { get; set; }
        public long PictureId { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<PictureResultModel> Pictures { get; set; }
        public ProductStatuses StatusId { get; set; }
        public IEnumerable<ProductAttributeRelationModel> ProductAttributes { get; set; }
    }
}
