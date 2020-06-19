using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Contract
{
    public interface IDataProvider
    {
        ITable<TEntity> GetTable<TEntity>() where TEntity : class;
        void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        object Insert<TEntity>(TEntity entity);
        Task<object> InsertAsync<TEntity>(TEntity entity);
        void Update<TEntity>(TEntity entity);
        Task UpdateAsync<TEntity>(TEntity entity);
        void Delete<TEntity>(TEntity entity);
        Task DeleteAsync<TEntity>(TEntity entity);
        void DeleteRange<TEntity>(IQueryable<TEntity> entities);
        Task DeleteRangeAsync<TEntity>(IQueryable<TEntity> entities);
        void UpdateByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class;
        Task UpdateByNameAsync<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class;
    }
}
