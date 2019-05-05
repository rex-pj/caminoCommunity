using Coco.Entities.Base;
using System.Collections.Generic;

namespace Coco.Entities.Domain.Account
{
    public class Gender : BaseEntity
    {
        public Gender()
        {
            this.UserInfos = new HashSet<UserInfo>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
