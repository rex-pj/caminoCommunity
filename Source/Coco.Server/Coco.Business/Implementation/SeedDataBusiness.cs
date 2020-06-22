using Coco.Business.Contracts;
using Coco.Entities.Dtos.General;
using Coco.IdentityDAL.Contracts;
using LinqToDB.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace Coco.Business.Implementation
{
    public class SeedDataBusiness : ISeedDataBusiness
    {
        private readonly IIdentityDataProvider _identityDataProvider;
        private readonly IConfiguration _configuration;
        public SeedDataBusiness(IIdentityDataProvider identityDataProvider, IConfiguration configuration)
        {
            _configuration = configuration;
            _identityDataProvider = identityDataProvider;
        }

        public bool IsDatabaseExist()
        {
            return _identityDataProvider.IsDatabaseExist();
        }

        public void SeedingIdentityDb(InstallationDto installationDto)
        {
            if (IsDatabaseExist())
            {
                return;
            }

            var builder = GetConnectionStringBuilder();
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

        private SqlConnectionStringBuilder GetConnectionStringBuilder()
        {
            var connectionString = _configuration.GetConnectionString("IdentityEntities");

            return new SqlConnectionStringBuilder(connectionString);
        }
    }
}
