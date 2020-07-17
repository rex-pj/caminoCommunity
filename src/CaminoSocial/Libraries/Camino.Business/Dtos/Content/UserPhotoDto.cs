namespace Camino.Business.Dtos.Content
{
    public class UserPhotoDto
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
