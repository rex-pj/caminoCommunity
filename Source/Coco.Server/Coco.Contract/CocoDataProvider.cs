using Coco.Contract.MapBuilder;
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
    public abstract class CocoDataProvider
    {
        private readonly DataConnection _dataConnection;
        protected MappingSchemaBuilder MappingSchemaBuilder { get; private set; }
        protected CocoDataProvider(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
            var fluentMappingBuilder = _dataConnection.MappingSchema.GetFluentMappingBuilder();
            if (Singleton<MappingSchemaBuilder>.Instance is null)
            {
                Singleton<MappingSchemaBuilder>.Instance = new MappingSchemaBuilder(fluentMappingBuilder);
                MappingSchemaBuilder = Singleton<MappingSchemaBuilder>.Instance;
                OnMappingSchemaCreating(MappingSchemaBuilder);
            }
        }

        protected virtual void OnMappingSchemaCreating(MappingSchemaBuilder mappingSchemaBuilder)
        {
            _dataConnection.AddMappingSchema(mappingSchemaBuilder.FluentMappingBuilder.MappingSchema);
        }

        public virtual DataConnectionTransaction BeginTransaction()
        {
            return _dataConnection.BeginTransaction();
        }

        public virtual async Task<DataConnectionTransaction> BeginTransactionAsync()
        {
            return await _dataConnection.BeginTransactionAsync();
        }

        public virtual void CommitTransaction()
        {
            _dataConnection.CommitTransaction();
        }

        public virtual async Task CommitTransactionAsync()
        {
            await _dataConnection.CommitTransactionAsync();
        }

        public virtual void RollbackTransaction()
        {
            _dataConnection.RollbackTransaction();
        }

        public virtual async Task RollbackTransactionAsync()
        {
            await _dataConnection.RollbackTransactionAsync();
        }

        public virtual ITable<TEntity> GetTable<TEntity>() where TEntity : class
        {
            return _dataConnection.GetTable<TEntity>();
        }

        public void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _dataConnection.BulkCopy(new BulkCopyOptions(), entities.RetrieveIdentity(_dataConnection));
        }

        public object Insert<TEntity>(TEntity entity)
        {
            var id = _dataConnection.Insert(entity);
            return id;
        }

        public async Task<object> InsertAsync<TEntity>(TEntity entity)
        {
            var id = await _dataConnection.InsertAsync(entity);
            return id;
        }

        public async Task<long> InsertWithInt64IdentityAsync<TEntity>(TEntity entity)
        {
            var id = await _dataConnection.InsertWithInt64IdentityAsync(entity);
            return id;
        }

        public long InsertWithInt64Identity<TEntity>(TEntity entity)
        {
            var id = _dataConnection.InsertWithInt64Identity(entity);
            return id;
        }

        public void Update<TEntity>(TEntity entity)
        {
            _dataConnection.Update(entity);
        }

        public async Task UpdateAsync<TEntity>(TEntity entity)
        {
            await _dataConnection.UpdateAsync(entity);
        }

        public void Delete<TEntity>(TEntity entity)
        {
            _dataConnection.Delete(entity);
        }

        public async Task DeleteAsync<TEntity>(TEntity entity)
        {
            await _dataConnection.DeleteAsync(entity);
        }

        public void DeleteRange<TEntity>(IQueryable<TEntity> entities)
        {
            entities.Delete();
        }

        public async Task DeleteRangeAsync<TEntity>(IQueryable<TEntity> entities)
        {
            await entities.DeleteAsync();
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
        public virtual void ValidatePropertyByName<TEntity>(TEntity entity, object value, string propertyName) where TEntity : class
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
        public virtual PropertyInfo GetPropertyInfoByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class
        {
            ValidatePropertyByName(entity, value, propertyName);

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

            return propertyInfo;
        }

        /// <summary>
        /// Set entity value by name
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void SetEntityValueByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class
        {
            var propertyInfo = GetPropertyInfoByName(entity, value, propertyName, isIgnoreCase);

            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyType), null);
        }
    }
}
