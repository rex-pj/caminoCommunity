using Camino.Core.Contracts.Repositories.Setup;
using Camino.Core.Util;
using Camino.Infrastructure.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Camino.Service.Repository.Setup
{
    public class DbCreationRepository : IDbCreationRepository
    {
        private readonly CaminoDataConnection _dataConnection;
        public DbCreationRepository(CaminoDataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public bool IsDatabaseExist()
        {
            return _dataConnection.IsDatabaseExist();
        }

        public async Task CreateDatabaseAsync(string sql)
        {
            if (IsDatabaseExist())
            {
                return;
            }

            await CreateDatabaseByScriptAsync();
            await CreateTablesByScriptAsync(sql);
        }

        private async Task CreateDatabaseByScriptAsync()
        {
            var builder = _dataConnection.GetConnectionStringBuilder();
            var databaseName = builder.InitialCatalog;
            builder.InitialCatalog = "master";
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var query = $"CREATE DATABASE [{databaseName}]";
                using (var command = new SqlCommand(query, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task CreateTablesByScriptAsync(string sql)
        {
            var builder = _dataConnection.GetConnectionStringBuilder();
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                var sqlCommands = SqlScriptUtil.GetCommandsFromScript(sql);
                foreach (var query in sqlCommands)
                {
                    var cmd = new SqlCommand(query, connection);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
