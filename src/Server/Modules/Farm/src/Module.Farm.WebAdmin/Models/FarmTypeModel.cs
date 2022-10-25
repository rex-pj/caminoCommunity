using Camino.Infrastructure.Identity.Models;
using Camino.Shared.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.Farm.WebAdmin.Models
{
    public class FarmTypeModel : BaseIdentityModel
    {
        public FarmTypeModel()
        {
            SelectFarmTypes = new List<SelectListItem>();
        }

        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public FarmTypeStatuses StatusId { get; set; }
        public IEnumerable<SelectListItem> SelectFarmTypes { get; set; }
    }
}
