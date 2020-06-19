using Coco.Contract;

namespace Coco.IdentityDAL
{
    public class IdentityDataProvider : DataProvider, IDataProvider
    {
        public IdentityDataProvider(IdentityDbConnection dataConnection) : base(dataConnection)
        {
        }
    }
}
