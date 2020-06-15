using Coco.Contract;

namespace Coco.IdentityDAL.Implementations
{
    public class EfIdentityRepository<TEntity> : CocoRepository<TEntity> where TEntity : class
    {
        public EfIdentityRepository(IdentityDbContext context) : base(context)
        {

        }
    }
}
