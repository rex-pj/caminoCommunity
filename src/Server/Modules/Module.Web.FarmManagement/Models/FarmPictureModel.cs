using Camino.Shared.Enums;
using Camino.Framework.Models;
using System;

namespace Module.Web.FarmManagement.Models
{
    public class FarmPictureModel : BaseModel
    {
        public long FarmId { get; set; }
        public string FarmName { get; set; }
        public FarmPictureTypes FarmPictureType { get; set; }
        public long PictureId { get; set; }
        public string PictureName { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset PictureUpdatedDate { get; set; }
        public long PictureUpdatedById { get; set; }
        public string PictureUpdatedBy { get; set; }
        public DateTimeOffset PictureCreatedDate { get; set; }
        public long PictureCreatedById { get; set; }
        public string PictureCreatedBy { get; set; }
    }
}
