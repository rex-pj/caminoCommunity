using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Content
{
    public class ArticleCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public DateTime UpdatedDate { get; set; }
        
        public long UpdatedById { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public long CreatedById { get; set; }

        public int? ParentId { get; set; }
        
        public virtual ArticleCategory ParentCategory { get; set; }

        public virtual ICollection<ArticleCategory> ChildCategories { get; set; }
    }
}
