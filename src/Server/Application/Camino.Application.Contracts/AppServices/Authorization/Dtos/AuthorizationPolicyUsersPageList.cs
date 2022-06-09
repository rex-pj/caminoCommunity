using Camino.Application.Contracts.AppServices.Users.Dtos;

namespace Camino.Application.Contracts.AppServices.Authorization.Dtos
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
