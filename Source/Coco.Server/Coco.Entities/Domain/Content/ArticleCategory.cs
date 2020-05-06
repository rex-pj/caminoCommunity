using Coco.Entities.Base;
using Coco.Entities.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Content
{
    [Table("Category", Schema = TableSchemaConst.CONTENT)]
    public class ArticleCategory : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }
        
        [MaxLength(1000)]
        public string Description { get; set; }
        
        public DateTime UpdatedDate { get; set; }
        
        [Required]
        public long UpdatedById { get; set; }
        
        public DateTime CreatedDate { get; set; }

        [Required]
        public long CreatedById { get; set; }

        [ForeignKey("ParentCategory")]
        public int? ParentCategoryId { get; set; }
        
        public virtual ArticleCategory ParentCategory { get; set; }

        [ForeignKey("ParentCategoryId")]
        public virtual ICollection<ArticleCategory> ChildCategories { get; set; }
    }
}
