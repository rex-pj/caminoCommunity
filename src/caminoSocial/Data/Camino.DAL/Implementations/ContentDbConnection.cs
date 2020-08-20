using Camino.DAL.Entities;
using Camino.Data.Contracts;
using LinqToDB;
using LinqToDB.Configuration;

namespace Camino.DAL.Implementations
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
