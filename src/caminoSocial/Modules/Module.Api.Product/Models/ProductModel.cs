using System;
using System.Collections;
using System.Collections.Generic;

namespace Module.Api.Product.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            Thumbnails = new List<PictureLoadModel>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedByIdentityId { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public IEnumerable<ProductCategoryProductModel> ProductCategories { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public IEnumerable<PictureLoadModel> Thumbnails { get; set; }
    }
}
