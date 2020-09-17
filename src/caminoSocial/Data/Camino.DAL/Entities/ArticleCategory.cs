using System;
using System.Collections.Generic;

namespace Camino.DAL.Entities
{
    public class ArticleCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public DateTimeOffset UpdatedDate { get; set; }
        
        public long UpdatedById { get; set; }
        
        public DateTimeOffset CreatedDate { get; set; }

        public long CreatedById { get; set; }

        public int? ParentId { get; set; }
        
        public virtual ArticleCategory ParentCategory { get; set; }

        public virtual ICollection<ArticleCategory> ChildCategories { get; set; }
    }
}
