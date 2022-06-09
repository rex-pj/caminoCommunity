namespace Camino.Core.Domains
{
    public interface IEntityRepository<TEntity> : IRepository<TEntity>, IAsyncRepository<TEntity>, IDisposable where TEntity : class
    {
    }
}
