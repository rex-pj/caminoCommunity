using Camino.Core.Contracts.Data;
using LinqToDB.Linq;

namespace Camino.Infrastructure.Linq2Db
{
    public class EntryUpdate<T> : IEntryUpdate<T>
    {
        public IUpdatable<T> Updatable;
        public EntryUpdate(IUpdatable<T> updateable)
        {
            Updatable = updateable;
        }
    }
}
