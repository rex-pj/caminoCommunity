using Camino.Data.Contracts;
using Camino.DAL.Contracts;

namespace Camino.DAL.Implementations
{
    public class ContentRepository<TEntity> : BaseRepository<TEntity> where TEntity : class
    {
        public ContentRepository(IContentDataProvider provider) : base(provider)
        {

        }
    }
}
