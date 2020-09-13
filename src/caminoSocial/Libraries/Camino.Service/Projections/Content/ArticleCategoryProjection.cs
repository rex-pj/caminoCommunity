using System;
using System.Collections.Generic;

namespace Camino.Service.Projections.Content
{
    public class ArticleCategoryProjection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public int? ParentId { get; set; }
        public string ParentCategoryName { get; set; }
        public ArticleCategoryProjection ParentCategory { get; set; }

        public IEnumerable<ArticleCategoryProjection> ChildCategories { get; set; }
    }
}
