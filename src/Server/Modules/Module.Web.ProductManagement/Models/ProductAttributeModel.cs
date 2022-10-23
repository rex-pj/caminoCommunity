using Camino.Infrastructure.Identity.Models;
using Camino.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.ProductManagement.Models
{
    public class ProductAttributeModel : BaseIdentityModel
    {
        public ProductAttributeModel()
        {
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public string Description { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ProductAttributeStatuses StatusId { get; set; }
    }
}
