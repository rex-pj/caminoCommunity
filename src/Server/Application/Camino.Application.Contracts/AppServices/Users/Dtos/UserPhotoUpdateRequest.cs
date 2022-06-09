namespace Camino.Application.Contracts.AppServices.Users.Dtos
{
    public class UserPhotoUpdateRequest
    {
        public string PhotoUrl { get; set; }
        public double XAxis { get; set; }
        public double YAxis { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Scale { get; set; }
        public string FileName { get; set; }
        public string UserPhotoCode { get; set; }
        public int UserPhotoTypeId { get; set; }
    }
}
