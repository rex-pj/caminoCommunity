using Camino.Data.Enums;
using Camino.Framework.Models;
using System;

namespace Module.Web.ArticleManagement.Models
{
    public class ArticlePictureModel : BaseModel
    {
        public long ArticleId { get; set; }
        public string ArticleName { get; set; }
        public ArticlePictureType ArticlePictureType { get; set; }
        public long PictureId { get; set; }
        public string PictureName { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset PictureUpdatedDate { get; set; }
        public long PictureUpdatedById { get; set; }
        public string PictureUpdatedBy { get; set; }
        public DateTimeOffset PictureCreatedDate { get; set; }
        public long PictureCreatedById { get; set; }
        public string PictureCreatedBy { get; set; }
    }
}
