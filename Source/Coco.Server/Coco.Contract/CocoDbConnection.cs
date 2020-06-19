using LinqToDB.Configuration;
using LinqToDB.Data;

namespace Coco.Contract
{
    public abstract class CocoDbConnection : DataConnection
    {
        protected CocoDbConnection(LinqToDbConnectionOptions options) : base(options)
        {

        }
    }
}
