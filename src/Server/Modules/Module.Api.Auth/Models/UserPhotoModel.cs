using Camino.Shared.Enums;

namespace Module.Api.Auth.Models
{
    public class UserPhotoModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public UserPictureType PhotoType { get; set; }
    }
}
