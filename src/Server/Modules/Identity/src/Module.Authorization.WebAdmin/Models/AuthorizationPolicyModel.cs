using Camino.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.Authorization.WebAdmin.Models
{
    public class AuthorizationPolicyModel : BaseIdentityModel
    {
        public AuthorizationPolicyModel()
        {
            SelectPermissionMethods = new List<SelectListItem>();
        }

        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public IEnumerable<SelectListItem> SelectPermissionMethods { get; set; }
        public int PermissionMethod { get; set; }

        public bool CanViewUserAuthorizationPolicy { get; set; }
        public bool CanViewRoleAuthorizationPolicy { get; set; }
    }
}
