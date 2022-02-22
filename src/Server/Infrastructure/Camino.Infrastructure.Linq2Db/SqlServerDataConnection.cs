using LinqToDB.Data;
using System;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using System.Data.SqlClient;
using LinqToDB.Configuration;
using System.Collections.Generic;
using LinqToDB.Tools;

namespace Camino.Infrastructure.Linq2Db
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
    }
}
