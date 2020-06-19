using Coco.Contract;
using Coco.Entities.Domain;

namespace Coco.IdentityDAL.Implementations
{
    public class IdentityRepository<TEntity> : CocoRepository<TEntity> where TEntity : class
    {
        public IdentityRepository(IdentityDataProvider provider) : base(provider)
        {

        }
    }
}
