using Camino.Shared.Requests.Authorization;
using Camino.Shared.Requests.Identifiers;
using Camino.Shared.Requests.Media;
using System.Collections.Generic;

namespace Camino.Shared.Requests.Setup
{
    public class SetupRequest
    {
        public UserModifyRequest InitualUser { get; set; }
        public IEnumerable<CountryModifyRequest> Countries { get; set; }
        public IEnumerable<GenderModifyRequest> Genders { get; set; }
        public IEnumerable<UserStatusModifyRequest> Statuses { get; set; }
        public IEnumerable<RoleModifyRequest> Roles { get; set; }
        public IEnumerable<AuthorizationPolicyRequest> AuthorizationPolicies { get; set; }
        public IEnumerable<UserPhotoTypeRequest> UserPhotoTypes { get; set; }
    }
}
