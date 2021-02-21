using HotChocolate;
using HotChocolate.Types;

namespace Camino.Framework.Models
{
    public class PictureRequestModel
    {
        [GraphQLType(typeof(LongType))]
        public long PictureId { get; set; }
        public string Base64Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
