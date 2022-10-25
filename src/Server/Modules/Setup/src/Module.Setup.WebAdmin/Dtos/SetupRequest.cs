using Camino.Application.Contracts.AppServices.Authorization.Dtos;
using Camino.Application.Contracts.AppServices.Identifiers.Dtos;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Application.Contracts.AppServices.Navigations.Dtos;
using Camino.Application.Contracts.AppServices.Users.Dtos;
using System.Collections.Generic;

namespace Module.Setup.WebAdmin.Dtos
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
        public IEnumerable<ShortcutModifyRequest> Shortcuts { get; set; }
    }
}
