using Camino.Shared.Enums;
using Camino.Infrastructure.Identity.Models;
using System;

namespace Module.Product.WebAdmin.Models
{
    public class ProductPictureModel : BaseIdentityModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public ProductPictureTypes ProductPictureType { get; set; }
        public long PictureId { get; set; }
        public string PictureName { get; set; }
        public string ContentType { get; set; }
        public DateTime PictureUpdatedDate { get; set; }
        public long PictureUpdatedById { get; set; }
        public string PictureUpdatedBy { get; set; }
        public DateTime PictureCreatedDate { get; set; }
        public long PictureCreatedById { get; set; }
        public string PictureCreatedBy { get; set; }
    }
}
