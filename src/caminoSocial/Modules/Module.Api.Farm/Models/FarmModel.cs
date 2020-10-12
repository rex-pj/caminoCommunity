using System;
using System.Collections.Generic;

namespace Module.Api.Farm.Models
{
    public class FarmModel
    {
        public FarmModel()
        {
            Thumbnails = new List<PictureLoadModel>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdateById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public string CreatedByIdentityId { get; set; }
        public long FarmTypeId { get; set; }
        public string FarmTypeName { get; set; }
        public IEnumerable<PictureLoadModel> Thumbnails { get; set; }
    }
}
