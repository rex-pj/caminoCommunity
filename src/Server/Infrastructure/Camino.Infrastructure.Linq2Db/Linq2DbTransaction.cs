using Camino.Core.Contracts.Data;
using LinqToDB.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Camino.Infrastructure.Linq2Db
{
    public class Linq2DbTransaction : IDatabaseTransaction, IDisposable, IAsyncDisposable
    {
        public DataConnectionTransaction DataConnectionTransaction;

        public Linq2DbTransaction(DataConnectionTransaction dataConnectionTransaction)
        {
            DataConnectionTransaction = dataConnectionTransaction ?? throw new ArgumentNullException(nameof(dataConnectionTransaction));
        }

        public void Commit()
        {
            DataConnectionTransaction.Commit();
            _disposed = false;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await DataConnectionTransaction.CommitAsync(cancellationToken);
            _disposed = false;
        }


        public void Rollback()
        {
            DataConnectionTransaction.Rollback();
            _disposed = false;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await DataConnectionTransaction.RollbackAsync(cancellationToken);
            _disposed = false;
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DataConnectionTransaction.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ValueTask DisposeAsync()
        {
            return DataConnectionTransaction.DisposeAsync();
        }
    }
}
