using Camino.Shared.Results.Identifiers;
using System.Collections.Generic;

namespace Camino.Shared.Results.PageList
{
    public class AuthorizationPolicyUsersPageList : BasePageList<UserResult>
    {
        public AuthorizationPolicyUsersPageList(IEnumerable<UserResult> collections) : base(collections)
        {

        }

        public AuthorizationPolicyUsersPageList() : base(new List<UserResult>())
        {

        }


        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
