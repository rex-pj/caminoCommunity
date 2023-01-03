namespace Camino.Core.Domains.Media
{
    public class UserPhoto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public byte[] FileData { get; set; }
        public long UserId { get; set; }
        public int TypeId { get; set; }
        public virtual UserPhotoType UserPhotoType { get; set; }
    }
}
