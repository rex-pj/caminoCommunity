namespace Camino.Service.Projections.Media
{
    public class UserPhotoProjection
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string ImageData { get; set; }
        public long UserId { get; set; }
        public byte TypeId { get; set; }
    }
}
