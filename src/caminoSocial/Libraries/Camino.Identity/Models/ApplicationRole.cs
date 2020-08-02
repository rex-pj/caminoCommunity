using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Camino.IdentityManager.Models
{
    public class ApplicationRole : IdentityRole<long>
    {
        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }

        public ApplicationRole()
        {
            AuthorizationPolicies = new List<ApplicationAuthorizationPolicy>();
        }

        public string Description { get; set; }
        public IEnumerable<ApplicationAuthorizationPolicy> AuthorizationPolicies { get; set; }
    }
}
