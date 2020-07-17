using System.Collections.Generic;

namespace Camino.Data.Entities.Identity
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
