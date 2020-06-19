using Coco.Contract;

namespace Coco.DAL
{
    public class ContentDbProvider : DbProvider
    {
        public ContentDbProvider(ContentDbConnection dataConnection) : base(dataConnection)
        {
        }
    }
}
