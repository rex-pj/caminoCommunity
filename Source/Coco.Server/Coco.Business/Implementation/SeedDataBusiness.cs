using Coco.Business.Contracts;
using Coco.IdentityDAL.Contracts;
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

        public bool CanSeed()
        {
            return !_identityDataProvider.IsDatabaseExist();
        }

        public void SeedingData()
        {
            if (!CanSeed())
            {
                return;
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("IdentityEntities")))
            {
                var query = $"CREATE DATABASE [Coco_IdentityDb1]";

                var command = new SqlCommand(query, connection);
                command.Connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}
