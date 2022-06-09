using Camino.Core.Domains.Farms;
using System;
using System.Collections.Generic;

namespace Camino.Core.Domains.Products
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public int StatusId { get; set; }
        public virtual ICollection<ProductCategoryRelation> ProductCategoryRelations { get; set; }
        public virtual ICollection<ProductPicture> ProductPictureRelations { get; set; }
        public virtual ICollection<ProductPrice> ProductPriceRelations { get; set; }
        public virtual ICollection<FarmProduct> ProductFarmRelations { get; set; }
    }
}
