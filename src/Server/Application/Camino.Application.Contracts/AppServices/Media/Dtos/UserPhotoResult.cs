namespace Camino.Application.Contracts.AppServices.Media.Dtos
{
    public class UserPhotoResult
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public int TypeId { get; set; }
        public byte[] FileData { get; set; }
    }
}
