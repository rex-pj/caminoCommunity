﻿using System.Collections.Generic;

namespace Coco.Core.Entities.Identity
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