using Camino.Data.Contracts;
using Camino.Service.Business.Setup.Contracts;
using LinqToDB.Data;
using System.Data.SqlClient;

namespace Camino.Service.Business.Setup
{
    public class SeedDataBusiness : ISeedDataBusiness
    {
        public void CreateDatabase(IBaseDataProvider dataProvider)
        {
            var builder = dataProvider.GetConnectionStringBuilder();
            var databaseName = builder.InitialCatalog;
            builder.InitialCatalog = "master";
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                var query = $"CREATE DATABASE [{databaseName}]";

                var command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void CreateDataByScript(IBaseDataProvider dataProvider, string sql)
        {
            using (var dataConnection = dataProvider.CreateDataConnection())
            {
                var sqlCommands = dataProvider.GetCommandsFromScript(sql);
                foreach (var command in sqlCommands)
                {
                    dataConnection.Execute(command);
                }
            }
        }
    }
}
