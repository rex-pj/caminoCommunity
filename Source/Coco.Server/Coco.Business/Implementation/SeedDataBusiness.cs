using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.DAL.Contracts;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.General;
using Coco.Entities.Dtos.User;
using Coco.IdentityDAL.Contracts;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Data.SqlClient;

namespace Coco.Business.Implementation
{
    public class SeedDataBusiness : ISeedDataBusiness
    {
        private readonly IIdentityDataProvider _identityDataProvider;
        private readonly IContentDataProvider _contentDataProvider;
        private readonly IMapper _mapper;
        public SeedDataBusiness(IIdentityDataProvider identityDataProvider, IContentDataProvider contentDataProvider, IMapper mapper)
        {
            _identityDataProvider = identityDataProvider;
            _contentDataProvider = contentDataProvider;
            _mapper = mapper;
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
            CreateData(_identityDataProvider, sql);
        }

        public void SeedingIdentityData(SetupDto installationDto, string sql)
        {
            CreateData(_identityDataProvider, sql);
        }

        public void SeedingContentDb(SetupDto installationDto, string sql)
        {
            if (IsContentDatabaseExist())
            {
                return;
            }

            CreateDatabase(_contentDataProvider);
            CreateData(_contentDataProvider, sql);
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

        private void CreateData(ICocoDataProvider dataProvider, string sql)
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

        public void CreateInitialUser(UserDto userDto)
        {
            using (var dataConnection = _identityDataProvider.CreateDataConnection())
            {
                using (var transaction = dataConnection.BeginTransaction())
                {
                    userDto.StatusId = 1;
                    userDto.IsActived = true;
                    userDto.CreatedDate = DateTime.UtcNow;
                    userDto.UpdatedDate = DateTime.UtcNow;

                    var user = _mapper.Map<User>(userDto);
                    var userInfo = _mapper.Map<UserInfo>(userDto);
                    try
                    {
                        var userId = dataConnection.InsertWithInt64Identity(user, nameof(User));
                        if (userId > 0)
                        {
                            userInfo.Id = userId;
                            dataConnection.InsertWithInt64Identity(userInfo, nameof(UserInfo));
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
