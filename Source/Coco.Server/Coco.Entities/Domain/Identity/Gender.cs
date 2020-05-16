using Coco.Entities.Constant;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Identity
{
    public class Gender
    {
        public Gender()
        {
            UserInfos = new HashSet<UserInfo>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
