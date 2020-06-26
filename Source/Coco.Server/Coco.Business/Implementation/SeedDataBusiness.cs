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

        public void SeedingIdentityDb(string sql)
        {
            if (IsIdentityDatabaseExist())
            {
                return;
            }

            CreateDatabase(_identityDataProvider);
            CreateDataByScript(_identityDataProvider, sql);
        }

        public void SeedingContentDb(string sql)
        {
            if (IsContentDatabaseExist())
            {
                return;
            }

            CreateDatabase(_contentDataProvider);
            CreateDataByScript(_contentDataProvider, sql);
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

        private void CreateDataByScript(ICocoDataProvider dataProvider, string sql)
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

        public void PrepareIdentityData(SetupDto installationDto)
        {
            using (var dataConnection = _identityDataProvider.CreateDataConnection())
            {
                using (var transaction = dataConnection.BeginTransaction())
                {
                    var statusTableName = nameof(Status);
                    // Insert user statuses
                    int activedStatusId = 0;
                    foreach(var statusDto in installationDto.Statuses)
                    {
                        var status = new Status()
                        {
                            Name = statusDto.Name,
                            Description = statusDto.Description
                        };

                        if (status.Name != "Actived")
                        {
                            dataConnection.Insert(status, statusTableName);
                        }
                        else
                        {
                            activedStatusId = dataConnection.Insert(status, statusTableName);
                        }
                    }

                    if(activedStatusId == 0)
                    {
                        return;
                    }
                    
                    // Insert genders
                    var genderTableName = nameof(Gender);
                    foreach (var gender in installationDto.Genders)
                    {
                        dataConnection.Insert(new Gender()
                        {
                            Name = gender.Name
                        }, genderTableName);
                    }

                    // Insert countries
                    var countryTableName = nameof(Country);
                    foreach (var country in installationDto.Countries)
                    {
                        dataConnection.Insert(new Country()
                        {
                            Name = country.Name,
                            Code = country.Code
                        }, countryTableName);
                    }

                    var userDto = installationDto.InitualUser;
                    var user = _mapper.Map<User>(userDto);
                    var userInfo = _mapper.Map<UserInfo>(userDto);
                    try
                    {
                        user.StatusId = activedStatusId;
                        user.IsActived = true;
                        user.CreatedDate = DateTime.UtcNow;
                        user.UpdatedDate = DateTime.UtcNow;
                        var userId = dataConnection.InsertWithInt64Identity(user, nameof(User));
                        if (userId > 0)
                        {
                            userInfo.Id = userId;
                            dataConnection.InsertWithInt64Identity(userInfo, nameof(UserInfo));

                            // Insert roles
                            var roleTableName = nameof(Role);
                            foreach (var role in installationDto.Roles)
                            {
                                dataConnection.Insert(new Role()
                                {
                                    Name = role.Name,
                                    Description = role.Description,
                                    CreatedById = userId,
                                    UpdatedById = userId,
                                }, roleTableName);
                            }
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
            _identityDataProvider.Dispose();
        }
    }
}
