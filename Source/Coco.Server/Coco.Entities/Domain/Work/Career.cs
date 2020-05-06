using Coco.Entities.Base;
using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Work
{
    [Table(nameof(Career), Schema = TableSchemaConst.DBO)]
    public class Career : BaseEntity
    {
        public Career()
        {
            UserCareers = new HashSet<UserCareer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("CreatedBy")]
        public long CreatedById { get; set; }
        public DateTime UpdatedDate { get; set; }

        [ForeignKey("UpdatedBy")]
        public long UpdatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }

        [ForeignKey("CareerId")]
        public virtual ICollection<UserCareer> UserCareers { get; set; }
    }
}
