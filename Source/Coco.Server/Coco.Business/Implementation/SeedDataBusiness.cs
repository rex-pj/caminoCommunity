using Coco.Business.Contracts;
using Coco.Contract;
using Coco.DAL.Contracts;
using Coco.Entities.Dtos.General;
using Coco.IdentityDAL.Contracts;
using LinqToDB.Data;
using System.Data.SqlClient;

namespace Coco.Business.Implementation
{
    public class SeedDataBusiness : ISeedDataBusiness
    {
        private readonly IIdentityDataProvider _identityDataProvider;
        private readonly IContentDataProvider _contentDataProvider;
        public SeedDataBusiness(IIdentityDataProvider identityDataProvider, IContentDataProvider contentDataProvider)
        {
            _identityDataProvider = identityDataProvider;
            _contentDataProvider = contentDataProvider;
        }

        public bool IsIdentityDatabaseExist()
        {
            return _identityDataProvider.IsDatabaseExist();
        }

        public bool IsContentDatabaseExist()
        {
            return _contentDataProvider.IsDatabaseExist();
        }

        public void SeedingIdentityDb(SetupDto installationDto, string sql)
        {
            if (IsIdentityDatabaseExist())
            {
                return;
            }

			CreateDatabase(_identityDataProvider);
            CreateTables(_identityDataProvider, sql);
        }

        public void SeedingContentDb(SetupDto installationDto, string sql)
        {
            if (IsContentDatabaseExist())
            {
                return;
            }

            CreateDatabase(_contentDataProvider);
            CreateTables(_contentDataProvider, sql);
        }

        private void CreateDatabase(ICocoDataProvider dataProvider)
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

        private void CreateTables(ICocoDataProvider dataProvider, string sql)
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
