using Coco.Contract;
using Coco.DAL.Contracts;

namespace Coco.DAL.Implementations
{
    public class ContentRepository<TEntity> : CocoRepository<TEntity> where TEntity : class
    {
        public ContentRepository(IContentDataProvider provider) : base(provider)
        {

        }
    }
}
