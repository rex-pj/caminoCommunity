using Camino.Service.Data.Page;
using System.Collections.Generic;

namespace Camino.Service.Data.Identity
{
    public class AuthorizationPolicyUsersResult : PageList<UserResult>
    {
        public AuthorizationPolicyUsersResult(IEnumerable<UserResult> collections) : base(collections)
        {

        }

        public AuthorizationPolicyUsersResult() : base(new List<UserResult>())
        {

        }


        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
