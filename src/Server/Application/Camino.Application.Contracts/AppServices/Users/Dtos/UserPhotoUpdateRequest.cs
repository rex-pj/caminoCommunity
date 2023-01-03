namespace Camino.Application.Contracts.AppServices.Users.Dtos
{
    public class UserPhotoUpdateRequest
    {
        public double XAxis { get; set; }
        public double YAxis { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Scale { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }
    }
}
