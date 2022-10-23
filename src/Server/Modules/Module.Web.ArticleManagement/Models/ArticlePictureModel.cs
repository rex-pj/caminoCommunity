using Camino.Shared.Enums;
using Camino.Infrastructure.Identity.Models;
using System;

namespace Module.Web.ArticleManagement.Models
{
    public class ArticlePictureModel : BaseIdentityModel
    {
        public long ArticleId { get; set; }
        public string ArticleName { get; set; }
        public ArticlePictureTypes ArticlePictureType { get; set; }
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
