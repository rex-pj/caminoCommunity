using Coco.Contract;

namespace Coco.DAL
{
    public class ContentDataProvider : DataProvider, IDataProvider
    {
        public ContentDataProvider(ContentDbConnection dataConnection) : base(dataConnection)
        {
        }
    }
}
