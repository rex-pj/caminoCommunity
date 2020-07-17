using Microsoft.AspNetCore.Identity;

namespace Camino.Framework.Models
{
    public class ApplicationUserRole : IdentityUserRole<long>
    {
        public string RoleName { get; set; }
    }
}
