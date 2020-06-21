using Coco.Contract;
using Coco.Entities.Domain.Content;
using LinqToDB;
using LinqToDB.Configuration;

namespace Coco.DAL
{
    public class ContentDbConnection : CocoDbConnection
    {
        public ITable<Product> Product { get; set; }
        public ITable<ArticleCategory> ArticleCategory { get; set; }
        public ITable<UserPhoto> UserPhoto { get; set; }
        public ITable<UserPhotoType> UserPhotoType { get; set; }

        public ContentDbConnection(LinqToDbConnectionOptions<ContentDbConnection> options) : base(options)
        {
        }
    }
}
