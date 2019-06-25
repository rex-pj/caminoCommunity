using Coco.Entities.Base;
using Coco.Entities.Domain.Account;
using System;
using System.Collections.Generic;

namespace Coco.Entities.Domain.Dbo
{
    public class Country : BaseEntity
    {
        public Country()
        {
            this.UserInfos = new HashSet<UserInfo>();
        }

        public short Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserInfo> UserInfos { get; set; }
    }
}
