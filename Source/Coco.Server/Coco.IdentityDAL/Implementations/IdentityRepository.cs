using Coco.Contract;

namespace Coco.IdentityDAL.Implementations
{
    public class IdentityRepository<TEntity> : CocoRepository<TEntity> where TEntity : class
    {
        public IdentityRepository(IdentityDbContext context) : base(context)
        {

        }
    }
}
