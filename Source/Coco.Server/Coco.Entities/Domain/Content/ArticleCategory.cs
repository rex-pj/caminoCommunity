using Coco.Entities.Base;
using System;

namespace Coco.Entities.Domain.Content
{
    public class ArticleCategory : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
