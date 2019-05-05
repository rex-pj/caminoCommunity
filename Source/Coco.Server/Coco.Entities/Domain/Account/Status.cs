using Coco.Entities.Base;
using System.Collections.Generic;

namespace Coco.Entities.Domain.Account
{
    public class Status : BaseEntity
    {
        public Status()
        {
            this.UserInfos = new HashSet<UserInfo>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
