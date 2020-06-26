using Coco.Contract.MapBuilder;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LinqToDB.Mapping;
using LinqToDB.DataProvider.SqlServer;
using LinqToDB.DataProvider;
using System.Text.RegularExpressions;
using System.Text;

namespace Coco.Contract
{
    public abstract class CocoDataProvider : IDisposable
    {
        private bool _disposed;
        private readonly DataConnection _dataConnection;
        private readonly IDataProvider _dataProvider;
        protected MappingSchemaBuilder MappingSchemaBuilder { get; private set; }
        protected CocoDataProvider(DataConnection dataConnection)
        {
            _dataProvider = new SqlServerDataProvider(ProviderName.SqlServer, SqlServerVersion.v2008);
            _dataConnection = dataConnection;
            var fluentMappingBuilder = _dataConnection.MappingSchema.GetFluentMappingBuilder();
            if (Singleton<MappingSchemaBuilder>.Instance is null)
            {
                Singleton<MappingSchemaBuilder>.Instance = new MappingSchemaBuilder(fluentMappingBuilder);
                MappingSchemaBuilder = Singleton<MappingSchemaBuilder>.Instance;
                OnMappingSchemaCreating(MappingSchemaBuilder);
                AllowMultipleQuery();
            }
        }

        public DataConnection CreateDataConnection()
        {
            var dataConnection = new DataConnection(_dataProvider, _dataConnection.ConnectionString);

            return dataConnection;
        }

        internal static void AllowMultipleQuery()
        {
            Configuration.Linq.AllowMultipleQuery = true;
        }

        protected virtual void OnMappingSchemaCreating(MappingSchemaBuilder builder)
        {
            _dataConnection.AddMappingSchema(builder.FluentMappingBuilder.MappingSchema);
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

        public SqlConnectionStringBuilder GetConnectionStringBuilder()
        {
            return new SqlConnectionStringBuilder(_dataConnection.ConnectionString);
        }

        public IList<string> GetCommandsFromScript(string sql)
        {
            var commands = new List<string>();

            //origin from the Microsoft.EntityFrameworkCore.Migrations.SqlServerMigrationsSqlGenerator.Generate method
            sql = Regex.Replace(sql, @"\\\r?\n", string.Empty, default, TimeSpan.FromMilliseconds(1000.0));
            var batches = Regex.Split(sql, @"^\s*(GO[ \t]+[0-9]+|GO)(?:\s+|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var batchLength = batches.Length;
            for (var i = 0; i < batchLength; i++)
            {
                if (string.IsNullOrWhiteSpace(batches[i]) || batches[i].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var count = 1;
                if (i != batches.Length - 1 && batches[i + 1].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                {
                    var match = Regex.Match(batches[i + 1], "([0-9]+)");
                    if (match.Success)
                    {
                        count = int.Parse(match.Value);
                    }
                }

                var builder = new StringBuilder();
                for (var j = 0; j < count; j++)
                {
                    builder.Append(batches[i]);
                    if (i == batches.Length - 1)
                    {
                        builder.AppendLine();
                    }
                }

                commands.Add(builder.ToString());
            }

            return commands;
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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                Singleton<MappingSchemaBuilder>.Instance = null;
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
