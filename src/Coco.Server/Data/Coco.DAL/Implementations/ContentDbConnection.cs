using Coco.Data.Contracts;
using Coco.Data.Entities.Content;
using LinqToDB;
using LinqToDB.Configuration;

namespace Coco.DAL.Implementations
{
    public class ContentDbConnection : BaseDataConnection
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
