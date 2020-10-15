using Camino.Service.Projections.Media;
using System;
using System.Collections.Generic;

namespace Camino.Service.Projections.Product
{
    public class ProductProjection
    {
        public ProductProjection()
        {
            Thumbnails = new List<PictureRequestProjection>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public int Price { get; set; }
        public IEnumerable<ProductCategoryProjection> ProductCategories { get; set; }
        public IEnumerable<PictureRequestProjection> Thumbnails { get; set; }
    }
}
