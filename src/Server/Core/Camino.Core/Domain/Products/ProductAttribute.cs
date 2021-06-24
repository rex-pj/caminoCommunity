using System;
using System.Collections.Generic;

namespace Camino.Core.Domain.Products
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public long CreatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public virtual ICollection<ProductAttributeRelation> ProductAttributeRelations { get; set; }
    }
}
