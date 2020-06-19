using Coco.Contract;
using Coco.Entities.Domain;

namespace Coco.IdentityDAL.Implementations
{
    public class IdentityRepository<TEntity> : CocoRepository<TEntity> where TEntity : BaseEntity
    {
        public IdentityRepository(IdentityDbProvider provider) : base(provider)
        {

        }
    }
}
