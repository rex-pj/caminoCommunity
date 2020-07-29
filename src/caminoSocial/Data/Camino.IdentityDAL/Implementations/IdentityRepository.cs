using Camino.Data.Contracts;
using Camino.IdentityDAL.Contracts;

namespace Camino.IdentityDAL.Implementations
{
    public class IdentityRepository<TEntity> : BaseRepository<TEntity> where TEntity : class
    {
        public IdentityRepository(IIdentityDataProvider provider) : base(provider)
        {

        }
    }
}
