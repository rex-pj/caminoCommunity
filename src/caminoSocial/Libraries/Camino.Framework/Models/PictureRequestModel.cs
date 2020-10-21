namespace Camino.Framework.Models
{
    public class PictureRequestModel
    {
        public long Id { get; set; }
        public string Base64Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
