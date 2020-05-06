using Coco.Entities.Base;
using Coco.Entities.Constant;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Identity
{
    [Table(nameof(Status), Schema = TableSchemaConst.DBO)]
    public class Status : BaseEntity
    {
        public Status()
        {
            Users = new HashSet<User>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("StatusId")]
        public virtual ICollection<User> Users { get; set; }
    }
}
