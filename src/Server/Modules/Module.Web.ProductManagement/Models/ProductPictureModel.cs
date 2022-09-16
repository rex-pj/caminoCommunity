using Camino.Shared.Enums;
using Camino.Framework.Models;
using System;

namespace Module.Web.ProductManagement.Models
{
    public class ProductPictureModel : BaseModel
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
