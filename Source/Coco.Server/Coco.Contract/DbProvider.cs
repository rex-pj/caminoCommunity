using Coco.Entities.Domain;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Coco.Contract
{
    public abstract class DbProvider
    {
        private readonly DataConnection _dataConnection;
        protected DbProvider(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public virtual ITable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity
        {
            return _dataConnection.GetTable<TEntity>();
        }

        public void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            _dataConnection.BulkCopy(new BulkCopyOptions(), entities.RetrieveIdentity(_dataConnection));
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            entity.Id = _dataConnection.Insert(entity);
            return entity;
        }

        public void Update<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _dataConnection.Update(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _dataConnection.Delete(entity);
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            await _dataConnection.DeleteAsync(entity);
        }

        public void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            if (entities.All(entity => entity.Id == 0))
            {
                foreach (var entity in entities)
                {
                    Delete(entity);
                }
            }
            else
            {
                GetTable<TEntity>()
                    .Where(e => e.Id.In(entities.Select(x => x.Id))).Delete();
            }
        }

        /// <summary>
        /// Update entity by Property Name
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void UpdateByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var type = entity.GetType();
            PropertyInfo propertyInfo;
            if (isIgnoreCase)
            {
                propertyInfo = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }
            else
            {
                propertyInfo = type.GetProperty(propertyName);
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException(nameof(propertyInfo));
            }

            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyType), null);

            _dataConnection.Update(entity);
        }
    }
}
