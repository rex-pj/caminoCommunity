using Coco.Entities.Constant;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Content
{
    [Table(nameof(UserCareer), Schema = TableSchemaConst.DBO)]
    public class UserCareer
    {
        [ForeignKey("Career")]
        public byte CareerId { get; set; }

        public long UserId { get; set; }

        public virtual Career Career { get; set; }
    }
}
