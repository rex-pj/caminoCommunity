using Coco.Contract;

namespace Coco.DAL.Implementations
{
    public class ContentRepository<TEntity> : CocoRepository<TEntity> where TEntity : class
    {
        public ContentRepository(ContentDbContext context) : base(context)
        {

        }
    }
}
