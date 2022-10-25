using Camino.Shared.Enums;
using Camino.Infrastructure.Identity.Models;
using System;

namespace Module.Farm.WebAdmin.Models
{
    public class FarmPictureModel : BaseIdentityModel
    {
        public long FarmId { get; set; }
        public string FarmName { get; set; }
        public FarmPictureTypes FarmPictureType { get; set; }
        public long PictureId { get; set; }
        public string PictureName { get; set; }
        public string ContentType { get; set; }
        public DateTime PictureUpdatedDate { get; set; }
        public long PictureUpdatedById { get; set; }
        public string PictureUpdatedBy { get; set; }
        public DateTime PictureCreatedDate { get; set; }
        public long PictureCreatedById { get; set; }
        public string PictureCreatedBy { get; set; }
    }
}
