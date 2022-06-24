using Camino.Core.Domains.Users;

namespace Camino.Core.Domains.Authorization
{
    public class UserRole
    {
        public long UserId { get; set; }
        
        public long RoleId { get; set; }
        public DateTime GrantedDate { get; set; }
        public long GrantedById { get; set; }
        public bool IsGranted { get; set; }

        public virtual User User { get; set; }
        public virtual User GrantedBy { get; set; }
        public virtual Role Role { get; set; }
    }
}
