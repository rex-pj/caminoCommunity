using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.DataProvider;
using System.Text.RegularExpressions;
using System.Text;
using LinqToDB.Mapping;
using Camino.Core.Infrastructure;
using System.Data.SqlClient;
using Camino.Data.Helpers;

namespace Camino.Data.Contracts
{
    public abstract class BaseDataProvider<TMappingSchema>
    {
        protected readonly DataConnection _dataConnection;
        private readonly IDataProvider _dataProvider;
        protected FluentMappingBuilder FluentMapBuilder { get; private set; }
        protected BaseDataProvider(DataConnection dataConnection)
        {
            _dataProvider = new SqlServerDataProvider(ProviderName.SqlServer, SqlServerVersion.v2008);
            _dataConnection = dataConnection;

            if (Singleton<TMappingSchema>.Instance == null)
            {
                LoadMappingSchemaBuilder();
            }

            OnMappingSchemaCreated();
        }

        protected void LoadMappingSchemaBuilder()
        {
            var mappingSchema = new MappingSchema();
            FluentMapBuilder = mappingSchema.GetFluentMappingBuilder();
            _dataConnection.AddMappingSchema(mappingSchema);
            OnMappingSchemaCreating();
            Singleton<MappingSchema>.Instance = _dataConnection.MappingSchema;
        }

        protected abstract void OnMappingSchemaCreating();

        protected void OnMappingSchemaCreated()
        {
            _dataConnection.AddMappingSchema(Singleton<MappingSchema>.Instance);
        }

        public DataConnection CreateDataConnection()
        {
            var mappingSchema = new MappingSchema();
            FluentMapBuilder = mappingSchema.GetFluentMappingBuilder();
            var dataConnection = new DataConnection(_dataProvider, _dataConnection.ConnectionString);
            dataConnection.AddMappingSchema(mappingSchema);
            OnMappingSchemaCreating();
            return dataConnection;
        }

        public bool IsDatabaseExist()
        {
            try
            {
                using (var connection = new SqlConnection(_dataConnection.ConnectionString))
                {
                    connection.Open();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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
            return new DataContext(_dataProvider, _dataConnection.ConnectionString) { MappingSchema = _dataConnection.MappingSchema }
                .GetTable<TEntity>();
            //return _dataConnection.GetTable<TEntity>();
        }

        public void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _dataConnection.BulkCopy(new BulkCopyOptions(), entities.RetrieveIdentity(_dataConnection));
        }

        public int Insert<TEntity>(TEntity entity)
        {
            var id = _dataConnection.Insert(entity);
            return id;
        }

        public async Task<int> InsertAsync<TEntity>(TEntity entity)
        {
            var id = await _dataConnection.InsertAsync(entity);
            return id;
        }

        public object InsertWithIdentity<TEntity>(TEntity entity)
        {
            var id = _dataConnection.InsertWithIdentity(entity);
            return id;
        }

        public async Task<object> InsertWithIdentityAsync<TEntity>(TEntity entity)
        {
            var id = await _dataConnection.InsertWithIdentityAsync(entity);
            return id;
        }

        public async Task<int> InsertWithInt32IdentityAsync<TEntity>(TEntity entity)
        {
            var id = await _dataConnection.InsertWithInt32IdentityAsync(entity);
            return id;
        }

        public int InsertWithInt32Identity<TEntity>(TEntity entity)
        {
            var id = _dataConnection.InsertWithInt32Identity(entity);
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

        public SqlConnectionStringBuilder GetConnectionStringBuilder()
        {
            return new SqlConnectionStringBuilder(_dataConnection.ConnectionString);
        }

        public IList<string> GetCommandsFromScript(string sql)
        {
            return SqlScriptHelper.GetCommandsFromScript(sql);
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
