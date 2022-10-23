namespace Camino.Infrastructure.AspNetCore.Models
{
    public class PictureRequestModel
    {
        public long? PictureId { get; set; }
        public string Base64Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
