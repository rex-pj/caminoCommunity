using Coco.Entities.Constant;
using Coco.Entities.Domain.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Dbo
{
    [Table(nameof(Country), Schema = TableSchemaConst.DBO)]
    public class Country
    {
        public Country()
        {
            this.UserInfos = new HashSet<UserInfo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        [ForeignKey("CountryId")]
        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
