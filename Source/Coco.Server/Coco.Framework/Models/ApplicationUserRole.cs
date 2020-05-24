using Microsoft.AspNetCore.Identity;

namespace Coco.Framework.Models
{
    public class ApplicationUserRole : IdentityUserRole<long>
    {
        public string RoleName { get; set; }
    }
}
