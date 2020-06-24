using LinqToDB.Configuration;
using LinqToDB.Data;

namespace Coco.Contract
{
    public abstract class BaseDataConnection : DataConnection
    {
        protected BaseDataConnection(LinqToDbConnectionOptions options) : base(options)
        {
        }
    }
}
