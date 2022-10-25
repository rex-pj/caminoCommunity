using Camino.Shared.Enums;

namespace Module.Auth.Api.Models
{
    public class UserPhotoModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public UserPictureTypes PhotoType { get; set; }
    }
}
