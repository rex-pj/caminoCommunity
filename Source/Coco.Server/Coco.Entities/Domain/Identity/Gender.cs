using Coco.Entities.Base;
using Coco.Entities.Constant;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Identity
{
    [Table(nameof(Gender), Schema = TableSchemaConst.DBO)]
    public class Gender : BaseEntity
    {
        public Gender()
        {
            UserInfos = new HashSet<UserInfo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("GenderId")]
        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
