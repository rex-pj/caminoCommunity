using System;

namespace Coco.Management.Models
{
    public class ArticleCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
