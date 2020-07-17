using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coco.Core.Entities.Identity
{
    public class Country
    {
        public Country()
        {
            this.UserInfos = new HashSet<UserInfo>();
        }

        public short Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
