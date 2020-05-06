using Coco.Entities.Base;
using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Dbo
{
    [Table(nameof(UserPhoto), Schema = TableSchemaConst.DBO)]
    public class UserPhoto : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string ImageData { get; set; }
        public long UserId { get; set; }
        public byte TypeId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
