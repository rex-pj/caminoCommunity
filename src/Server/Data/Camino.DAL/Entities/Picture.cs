using System;

namespace Camino.DAL.Entities
{
    public class Picture
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
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        public int StatusId { get; set; }
    }
}
