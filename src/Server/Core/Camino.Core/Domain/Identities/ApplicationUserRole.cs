using Microsoft.AspNetCore.Identity;

namespace Camino.Core.Domain.Identities
{
    public class ApplicationUserRole : IdentityUserRole<long>
    {
        public string RoleName { get; set; }
    }
}
