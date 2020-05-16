using Coco.Entities.Domain.Identity;

namespace Coco.Entities.Domain.Auth
{
    public class UserRole
    {
        public long UserId { get; set; }
        
        public byte RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
