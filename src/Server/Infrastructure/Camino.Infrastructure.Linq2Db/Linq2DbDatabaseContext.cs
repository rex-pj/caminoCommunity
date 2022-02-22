using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.DependencyInjection;
using LinqToDB;
using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace Camino.Infrastructure.Linq2Db
{
    public class Linq2DbDatabaseContext : IDatabaseContext, IDisposable, IScopedDependency
    {
        private readonly CaminoDataConnection _dataConnection;
        public Linq2DbDatabaseContext(CaminoDataConnection dataConnection)
        {
            _dataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
        }

        public virtual IDatabaseTransaction BeginTransaction()
        {
            return new Linq2DbTransaction(_dataConnection.BeginTransaction());
        }

        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new Linq2DbTransaction(_dataConnection.BeginTransaction(isolationLevel));
        }

        /// <summary>
        /// Update entity by Property Name
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void UpdateByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class
        {
            SetEntityValueByName(entity, value, propertyName, isIgnoreCase);
            _dataConnection.Update(entity);
        }

        /// <summary>
        /// Update entity by Property Name
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task UpdateByNameAsync<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class
        {
            SetEntityValueByName(entity, value, propertyName, isIgnoreCase);
            await _dataConnection.UpdateAsync(entity);
        }

        /// <summary>
        /// Validate property by name
        /// </summary>
        /// <param name="entity">Entity</param>
        private void ValidatePropertyByName<TEntity>(TEntity entity, object value, string propertyName) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
        }

        /// <summary>
        /// Set entity value by name
        /// </summary>
        /// <param name="entity">Entity</param>
        private PropertyInfo GetPropertyInfoByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class
        {
            ValidatePropertyByName(entity, value, propertyName);
            if (isIgnoreCase)
            {
                return entity.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }

            return entity.GetType().GetProperty(propertyName);
        }

        /// <summary>
        /// Set entity value by name
        /// </summary>
        /// <param name="entity">Entity</param>
        private void SetEntityValueByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class
        {
            var propertyInfo = GetPropertyInfoByName(entity, value, propertyName, isIgnoreCase);
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyType), null);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dataConnection.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
