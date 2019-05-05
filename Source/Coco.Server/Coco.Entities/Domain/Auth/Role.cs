using Coco.Entities.Base;
using System.Collections.Generic;

namespace Coco.Entities.Domain.Auth
{
    public class Role : BaseEntity
    {
        public Role()
        {
            this.UserRoles = new HashSet<UserRole>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
