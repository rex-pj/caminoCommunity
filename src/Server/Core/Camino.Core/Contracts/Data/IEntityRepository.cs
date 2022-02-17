using System;

namespace Camino.Core.Contracts.Data
{
    public interface IEntityRepository<TEntity> : IRepository<TEntity>, IAsyncRepository<TEntity>, IDisposable where TEntity : class
    {
    }
}
