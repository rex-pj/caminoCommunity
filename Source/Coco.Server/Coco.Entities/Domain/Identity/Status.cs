using System.Collections.Generic;

namespace Coco.Entities.Domain.Identity
{
    public class Status
    {
        public Status()
        {
            Users = new HashSet<User>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
