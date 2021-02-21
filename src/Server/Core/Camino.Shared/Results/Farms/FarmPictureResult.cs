using System;

namespace Camino.Shared.Results.Farms
{
    public class FarmPictureResult
    {
        public long FarmId { get; set; }
        public string FarmName { get; set; }
        public int FarmPictureTypeId { get; set; }
        public long PictureId { get; set; }
        public string PictureName { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset PictureCreatedDate { get; set; }
        public long PictureCreatedById { get; set; }
        public string PictureCreatedBy { get; set; }
    }
}
