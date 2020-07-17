using LinqToDB.Configuration;
using LinqToDB.Data;

namespace Coco.Data.Contracts
{
    public abstract class BaseDataConnection : DataConnection
    {
        protected BaseDataConnection(LinqToDbConnectionOptions options) : base(options)
        {
        }
    }
}
