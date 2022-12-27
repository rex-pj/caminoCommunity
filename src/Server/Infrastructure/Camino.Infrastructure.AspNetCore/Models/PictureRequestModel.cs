using Microsoft.AspNetCore.Http;

namespace Camino.Infrastructure.AspNetCore.Models
{
    public class PictureRequestModel
    {
        public long? PictureId { get; set; }
        public IFormFile File { get; set; }
    }
}
