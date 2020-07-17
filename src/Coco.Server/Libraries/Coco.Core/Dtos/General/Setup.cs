using Coco.Core.Dtos.Identity;
using Coco.Core.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Coco.Core.Dtos.General
{
    public class Setup
    {
        public string AdminEmail { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public DateTime BirthDate { get; set; }
        public string AdminPassword { get; set; }
        public string AdminConfirmPassword { get; set; }
        public UserDto InitualUser { get; set; }
        public IEnumerable<CountryDto> Countries { get; set; }
        public IEnumerable<GenderDto> Genders { get; set; }
        public IEnumerable<StatusDto> Statuses { get; set; }
        public IEnumerable<RoleDto> Roles { get; set; }
        public IEnumerable<AuthorizationPolicy> AuthorizationPolicies { get; set; }
    }
}
