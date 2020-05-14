using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Coco.Framework.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
        {
            AuthorizationPolicies = new List<ApplicationAuthorizationPolicy>();
        }

        public string Description { get; set; }
        public IEnumerable<ApplicationAuthorizationPolicy> AuthorizationPolicies { get; set; }
    }
}
