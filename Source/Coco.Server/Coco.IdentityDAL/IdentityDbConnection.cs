using Coco.Contract;
using LinqToDB.Configuration;

namespace Coco.IdentityDAL
{
    public class IdentityDbConnection : CocoDbConnection
    {
        public IdentityDbConnection(LinqToDbConnectionOptions<IdentityDbConnection> options) : base(options)
        {
        }
    }
}
