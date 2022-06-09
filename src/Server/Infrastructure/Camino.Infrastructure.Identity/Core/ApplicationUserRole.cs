using Microsoft.AspNetCore.Identity;

namespace Camino.Infrastructure.Identity.Core
{
    public class ApplicationUserRole : IdentityUserRole<long>
    {
        public string RoleName { get; set; }
    }
}
