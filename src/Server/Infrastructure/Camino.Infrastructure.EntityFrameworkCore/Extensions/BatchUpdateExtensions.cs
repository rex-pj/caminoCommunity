using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Camino.Infrastructure.EntityFrameworkCore.Extensions
{
    public static class BatchUpdateExtensions
    {
        public static BatchUpdate<TEntity> BatchUpdate<TEntity>(this IQueryable<TEntity> source) where TEntity : class
        {
            BatchUpdate<TEntity> builder = new BatchUpdate<TEntity>(source as DbSet<TEntity>);
            return builder;
        }

        public static BatchUpdate<TEntity> BatchUpdate<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class
        {
            BatchUpdate<TEntity> builder = new BatchUpdate<TEntity>(dbSet);
            return builder;
        }
    }
}
