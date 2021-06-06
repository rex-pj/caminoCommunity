using Camino.Shared.Results.Media;
using System;
using System.Collections.Generic;

namespace Camino.Shared.Results.Products
{
    public class ProductResult
    {
        public ProductResult()
        {
            Pictures = new List<PictureResult>();
            Categories = new List<ProductCategoryResult>();
            Farms = new List<ProductFarmResult>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public decimal Price { get; set; }
        public int StatusId { get; set; }
        public IEnumerable<ProductCategoryResult> Categories { get; set; }
        public IEnumerable<ProductFarmResult> Farms { get; set; }
        public IEnumerable<PictureResult> Pictures { get; set; }
        public IEnumerable<ProductAttributeRelationResult> ProductAttributes { get; set; }
    }
}
