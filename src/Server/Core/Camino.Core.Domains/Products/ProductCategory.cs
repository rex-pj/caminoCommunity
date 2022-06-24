using System;
using System.Collections.Generic;

namespace Camino.Core.Domains.Products
{
    public class ProductCategory
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public long UpdatedById { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public long CreatedById { get; set; }

        public long? ParentId { get; set; }
        public int StatusId { get; set; }

        public virtual ProductCategory ParentCategory { get; set; }

        public virtual ICollection<ProductCategory> ChildCategories { get; set; }
        public virtual ICollection<ProductCategoryRelation> ProductCategoryRelations { get; set; }
    }
}
