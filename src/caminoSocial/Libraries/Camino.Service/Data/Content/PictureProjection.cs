using System;

namespace Camino.Service.Data.Content
{
    public class PictureProjection
    {
        public long Id { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }
        public byte[] BinaryData { get; set; }
        public string RelativePath { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
    }
}
