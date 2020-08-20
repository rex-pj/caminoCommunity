using Camino.Service.Data.Content;
using Camino.Service.Data.Identity;
using System;
using System.Collections.Generic;

namespace Camino.Service.Data.Request
{
    public class SetupRequest
    {
        public string AdminEmail { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public DateTime BirthDate { get; set; }
        public string AdminPassword { get; set; }
        public string AdminConfirmPassword { get; set; }
        public UserResult InitualUser { get; set; }
        public IEnumerable<CountryResult> Countries { get; set; }
        public IEnumerable<GenderResult> Genders { get; set; }
        public IEnumerable<UserStatusResult> Statuses { get; set; }
        public IEnumerable<RoleResult> Roles { get; set; }
        public IEnumerable<AuthorizationPolicyResult> AuthorizationPolicies { get; set; }
        public IEnumerable<UserPhotoTypeResult> UserPhotoTypes { get; set; }
    }
}
