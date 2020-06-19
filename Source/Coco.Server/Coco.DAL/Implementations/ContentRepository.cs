using Coco.Contract;
using Coco.Entities.Domain;

namespace Coco.DAL.Implementations
{
    public class ContentRepository<TEntity> : CocoRepository<TEntity> where TEntity : BaseEntity
    {
        public ContentRepository(ContentDbProvider provider) : base(provider)
        {

        }
    }
}
