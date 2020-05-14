using Microsoft.AspNetCore.Identity;

namespace Coco.Framework.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
