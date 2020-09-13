namespace Camino.Service.Projections.Media
{
    public class PictureLoadProjection
    {
        public long Id { get; set; }
        public string Base64Data { get; set; }
        public byte[] BinaryData { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
