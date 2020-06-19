using Coco.Contract;

namespace Coco.IdentityDAL
{
    public class IdentityDbProvider : DbProvider
    {
        public IdentityDbProvider(IdentityDbConnection dataConnection) : base(dataConnection)
        {
        }
    }
}
