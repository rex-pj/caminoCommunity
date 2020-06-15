using Coco.Contract;

namespace Coco.DAL.Implementations
{
    public class EfRepository<TEntity> : CocoRepository<TEntity> where TEntity : class
    {
        public EfRepository(ContentDbContext context) : base(context)
        {

        }
    }
}
