using Coco.Entities.Base;
using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Auth
{
    [Table(nameof(UserRole), Schema = TableSchemaConst.DBO)]
    public class UserRole : BaseEntity
    {
        [ForeignKey("User")]
        public long UserId { get; set; }
        
        [ForeignKey("Role")]
        public byte RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
