using LinqToDB;
using LinqToDB.Data;
using System;
using System.Reflection;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using System.Data.SqlClient;
using LinqToDB.Configuration;

namespace Camino.Infrastructure.Data
{
    public abstract class SqlServerDataConnection : DataConnection
    {
        protected FluentMappingBuilder FluentMapBuilder { get; private set; }
        protected SqlServerDataConnection(LinqToDbConnectionOptions<CaminoDataConnection> options) : base(options)
        {
            var mappingSchema = new MappingSchema();
            FluentMapBuilder = mappingSchema.GetFluentMappingBuilder();
            AddMappingSchema(mappingSchema);
            OnMappingSchemaCreating();
        }

        protected abstract void OnMappingSchemaCreating();

        public virtual bool IsDatabaseExist()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual SqlConnectionStringBuilder GetConnectionStringBuilder()
        {
            return new SqlConnectionStringBuilder(ConnectionString);
        }

        /// <summary>
        /// Update entity by Property Name
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void UpdateByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class
        {
            SetEntityValueByName(entity, value, propertyName, isIgnoreCase);
            this.Update(entity);
        }

        /// <summary>
        /// Update entity by Property Name
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task UpdateByNameAsync<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class
        {
            SetEntityValueByName(entity, value, propertyName, isIgnoreCase);
            await this.UpdateAsync(entity);
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
        public virtual void SetEntityValueByName<TEntity>(TEntity entity, object value, string propertyName, bool isIgnoreCase = false) where TEntity : class
        {
            var propertyInfo = GetPropertyInfoByName(entity, value, propertyName, isIgnoreCase);
            if(propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyType), null);
        }
    }
}
