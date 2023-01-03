namespace Camino.Core.Domains.Media
{
    public class Picture
    {
        public long Id { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }
        public byte[] BinaryData { get; set; }
        public string RelativePath { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public int StatusId { get; set; }
    }
}
