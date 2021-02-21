using Camino.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.Web.FarmManagement.Models
{
    public class FarmModel : BaseModel
    {
        public FarmModel()
        {
            Pictures = new List<PictureRequestModel>();
        }

        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public long FarmTypeId { get; set; }
        public string FarmTypeName { get; set; }
        public IEnumerable<PictureRequestModel> Pictures { get; set; }
        public long PictureId { get; set; }
        public IFormFile File { get; set; }

        public IEnumerable<SelectListItem> SelectTypes { get; set; }
    }
}
