using Microsoft.AspNetCore.Identity;

namespace Camino.IdentityManager.Models
{
    public class ApplicationUserRole : IdentityUserRole<long>
    {
        public string RoleName { get; set; }
    }
}
