using Coco.Contract;
using LinqToDB.Configuration;

namespace Coco.DAL
{
    public class ContentDbConnection : CocoDbConnection
    {
        public ContentDbConnection(LinqToDbConnectionOptions options) : base(options)
        {
        }
    }
}
