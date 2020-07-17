using Coco.Data.Contracts;
using Coco.IdentityDAL.Contracts;

namespace Coco.IdentityDAL.Implementations
{
    public class IdentityRepository<TEntity> : CocoRepository<TEntity> where TEntity : class
    {
        public IdentityRepository(IIdentityDataProvider provider) : base(provider)
        {

        }
    }
}
