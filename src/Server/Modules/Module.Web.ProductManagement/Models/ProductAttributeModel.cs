using Camino.Framework.Models;
using Camino.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.ProductManagement.Models
{
    public class ProductAttributeModel : BaseModel
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
        public DateTimeOffset CreatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public ProductAttributeStatuses StatusId { get; set; }
    }
}
