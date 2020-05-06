using Coco.Entities.Base;
using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Work
{
    [Table(nameof(UserCareer), Schema = TableSchemaConst.DBO)]
    public class UserCareer : BaseEntity
    {
        [ForeignKey("Career")]
        public byte CareerId { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        public virtual Career Career { get; set; }
        public virtual User User { get; set; }
    }
}
