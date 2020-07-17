using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.AuthorizationManagement.Models
{
    public class AuthorizationPolicyViewModel
    {
        public AuthorizationPolicyViewModel()
        {
            SelectPermissionMethods = new List<SelectListItem>();
        }

        public int Id { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }
        
        [MaxLength(1000)]
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public IEnumerable<SelectListItem> SelectPermissionMethods { get; set; }
        public int PermissionMethod { get; set; }
    }
}
