using Camino.Infrastructure.AspNetCore.Models;
using Camino.Infrastructure.Identity.Models;
using Camino.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Module.Web.FarmManagement.Models
{
    public class FarmModel : BaseIdentityModel
    {
        public FarmModel()
        {
            Pictures = new List<PictureRequestModel>();
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public long FarmTypeId { get; set; }
        public string FarmTypeName { get; set; }
        public IEnumerable<PictureRequestModel> Pictures { get; set; }
        public long PictureId { get; set; }
        public FarmStatuses StatusId { get; set; }
    }
}
