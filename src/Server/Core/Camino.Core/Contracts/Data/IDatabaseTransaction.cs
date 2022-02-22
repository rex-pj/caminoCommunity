using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Data
{
    public interface IDatabaseTransaction : IDisposable, IAsyncDisposable
    {
        public void Commit();
        public void Rollback();
        public Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
        public Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
