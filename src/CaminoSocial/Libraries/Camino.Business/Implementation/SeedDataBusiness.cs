using AutoMapper;
using Camino.Business.Contracts;
using Camino.Data.Contracts;
using Camino.DAL.Contracts;
using Camino.Business.Dtos.General;
using Camino.IdentityDAL.Contracts;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Data.SqlClient;
using Camino.Data.Entities.Identity;

namespace Camino.Business.Implementation
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

        private void CreateDatabase(IBaseDataProvider dataProvider)
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

        private void CreateDataByScript(IBaseDataProvider dataProvider, string sql)
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
                    foreach (var statusDto in installationDto.Statuses)
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

                    if (activedStatusId == 0)
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

                    // Insert user
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
                            var adminRoleId = 0;
                            foreach (var role in installationDto.Roles)
                            {
                                var newRole = new Role()
                                {
                                    Name = role.Name,
                                    Description = role.Description,
                                    CreatedById = userId,
                                    UpdatedById = userId,
                                    CreatedDate = DateTime.UtcNow,
                                    UpdatedDate = DateTime.UtcNow
                                };

                                if (role.Name == "Admin")
                                {
                                    adminRoleId = dataConnection.Insert(newRole, roleTableName);
                                }
                                else
                                {
                                    dataConnection.Insert(newRole, roleTableName);
                                }
                            }

                            if (adminRoleId > 0)
                            {
                                // Insert user role
                                var adminUserRole = new UserRole()
                                {
                                    UserId = userId,
                                    GrantedById = userId,
                                    GrantedDate = DateTime.UtcNow,
                                    IsGranted = true,
                                    RoleId = adminRoleId
                                };

                                var userRoleTableName = nameof(UserRole);
                                dataConnection.Insert(adminUserRole, userRoleTableName);
                            }

                            // Insert authorization policies
                            var authorizationPolicyTableName = nameof(AuthorizationPolicy);
                            foreach (var authorizationPolicy in installationDto.AuthorizationPolicies)
                            {
                                var authorizationPolicyId = dataConnection.InsertWithInt64Identity(new AuthorizationPolicy()
                                {
                                    Name = authorizationPolicy.Name,
                                    Description = authorizationPolicy.Description,
                                    CreatedById = userId,
                                    CreatedDate = DateTime.UtcNow,
                                    UpdatedById = userId,
                                    UpdatedDate = DateTime.UtcNow
                                }, authorizationPolicyTableName);

                                var roleAuthorizationPolicyTableName = nameof(RoleAuthorizationPolicy);
                                dataConnection.Insert(new RoleAuthorizationPolicy()
                                {
                                    GrantedById = userId,
                                    GrantedDate = DateTime.UtcNow,
                                    IsGranted = true,
                                    RoleId = adminRoleId,
                                    AuthorizationPolicyId = authorizationPolicyId
                                }, roleAuthorizationPolicyTableName);

                                var userAuthorizationPolicyTableName = nameof(UserAuthorizationPolicy);
                                dataConnection.Insert(new UserAuthorizationPolicy()
                                {
                                    GrantedById = userId,
                                    GrantedDate = DateTime.UtcNow,
                                    IsGranted = true,
                                    UserId = userId,
                                    AuthorizationPolicyId = authorizationPolicyId
                                }, userAuthorizationPolicyTableName);
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
        }
    }
}
