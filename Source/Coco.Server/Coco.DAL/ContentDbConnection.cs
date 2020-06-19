using Coco.Contract;
using LinqToDB.Configuration;

namespace Coco.DAL
{
    public class ContentDbConnection : CocoDbConnection
    {
        public ContentDbConnection(LinqToDbConnectionOptions<ContentDbConnection> options) : base(options)
        {
        }
    }
}
