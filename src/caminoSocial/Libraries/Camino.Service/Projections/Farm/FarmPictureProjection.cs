using System;

namespace Camino.Service.Projections.Content
{
    public class FarmPictureProjection
    {
        public long FarmId { get; set; }
        public string FarmName { get; set; }
        public int FarmPictureType { get; set; }
        public long PictureId { get; set; }
        public string PictureName { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset PictureCreatedDate { get; set; }
        public long PictureCreatedById { get; set; }
        public string PictureCreatedBy { get; set; }
    }
}
