using Camino.Service.Projections.Identity;
using Camino.Service.Projections.Media;
using System;
using System.Collections.Generic;

namespace Camino.Service.Projections.Request
{
    public class SetupRequest
    {
        public string AdminEmail { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public DateTime BirthDate { get; set; }
        public string AdminPassword { get; set; }
        public string AdminConfirmPassword { get; set; }
        public UserProjection InitualUser { get; set; }
        public IEnumerable<CountryProjection> Countries { get; set; }
        public IEnumerable<GenderProjection> Genders { get; set; }
        public IEnumerable<UserStatusProjection> Statuses { get; set; }
        public IEnumerable<RoleProjection> Roles { get; set; }
        public IEnumerable<AuthorizationPolicyProjection> AuthorizationPolicies { get; set; }
        public IEnumerable<UserPhotoTypeProjection> UserPhotoTypes { get; set; }
    }
}
