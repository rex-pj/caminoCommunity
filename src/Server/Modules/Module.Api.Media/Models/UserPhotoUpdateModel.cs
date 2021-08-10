using Camino.Shared.Enums;
using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Media.Models
{
    public class UserPhotoUpdateModel
    {
        public string PhotoUrl { get; set; }
        public double XAxis { get; set; }
        public double YAxis { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Scale { get; set; }
        public string ContentType { get; set; }
        public bool CanEdit { get; set; }
        public string FileName { get; set; }
        public string UserPhotoCode { get; set; }
        [GraphQLType(typeof(IntType))]
        public UserPictureType UserPhotoType { get; set; }
    }
}
