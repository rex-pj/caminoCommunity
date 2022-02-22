using System;
using System.Data;
using System.Threading.Tasks;

namespace Camino.Core.Contracts.Data
{
    public interface IDatabaseContext : IDisposable
    {
        IDatabaseTransaction BeginTransaction();
        IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel);
        void UpdateByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class;
        Task UpdateByNameAsync<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class;
    }
}
