using AutoMapper;
using Camino.Data.Contracts;
using Camino.DAL.Contracts;
using Camino.IdentityDAL.Contracts;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Camino.Data.Enums;
using Camino.Service.Business.Setup.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.DAL.Entities;
using Camino.Service.Projections.Request;

namespace Camino.Service.Business.Setup
{
    public class SetupBusiness : ISetupBusiness
    {
        private readonly IIdentityDataProvider _identityDataProvider;
        private readonly IContentDataProvider _contentDataProvider;
        private readonly IMapper _mapper;
        public SetupBusiness(IIdentityDataProvider identityDataProvider, IContentDataProvider contentDataProvider, IMapper mapper)
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

        public async Task PrepareIdentityDataAsync(SetupRequest installationRequest)
        {
            using (var dataConnection = _identityDataProvider.CreateDataConnection())
            {
                using (var transaction = await dataConnection.BeginTransactionAsync())
                {
                    var statusTableName = nameof(Status);
                    // Insert user statuses
                    int activedStatusId = 0;
                    foreach (var statusRequest in installationRequest.Statuses)
                    {
                        var status = new Status()
                        {
                            Name = statusRequest.Name,
                            Description = statusRequest.Description
                        };

                        if (status.Name == UserStatus.Actived.ToString())
                        {
                            activedStatusId = await dataConnection.InsertWithInt32IdentityAsync(status, statusTableName);
                        }
                        else
                        {
                            await dataConnection.InsertWithInt32IdentityAsync(status, statusTableName);
                        }
                    }

                    if (activedStatusId == 0)
                    {
                        return;
                    }

                    // Insert genders
                    var genderTableName = nameof(Gender);
                    foreach (var gender in installationRequest.Genders)
                    {
                        await dataConnection.InsertAsync(new Gender()
                        {
                            Name = gender.Name
                        }, genderTableName);
                    }

                    // Insert countries
                    var countryTableName = nameof(Country);
                    foreach (var country in installationRequest.Countries)
                    {
                        await dataConnection.InsertAsync(new Country()
                        {
                            Name = country.Name,
                            Code = country.Code
                        }, countryTableName);
                    }

                    // Insert user
                    var userRequest = installationRequest.InitualUser;
                    var user = _mapper.Map<User>(userRequest);
                    var userInfo = _mapper.Map<UserInfo>(userRequest);
                    try
                    {
                        user.StatusId = activedStatusId;
                        user.IsEmailConfirmed = true;
                        user.CreatedDate = DateTime.UtcNow;
                        user.UpdatedDate = DateTime.UtcNow;
                        var userId = await dataConnection.InsertWithInt64IdentityAsync(user, nameof(User));
                        if (userId > 0)
                        {
                            userInfo.Id = userId;
                            await dataConnection.InsertWithInt64IdentityAsync(userInfo, nameof(UserInfo));

                            // Insert roles
                            var roleTableName = nameof(Role);
                            var adminRoleId = 0;
                            foreach (var role in installationRequest.Roles)
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
                                    adminRoleId = await dataConnection.InsertAsync(newRole, roleTableName);
                                }
                                else
                                {
                                    await dataConnection.InsertAsync(newRole, roleTableName);
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
                                await dataConnection.InsertAsync(adminUserRole, userRoleTableName);
                            }

                            // Insert authorization policies
                            var authorizationPolicyTableName = nameof(AuthorizationPolicy);
                            foreach (var authorizationPolicy in installationRequest.AuthorizationPolicies)
                            {
                                var authorizationPolicyId = await dataConnection.InsertWithInt64IdentityAsync(new AuthorizationPolicy()
                                {
                                    Name = authorizationPolicy.Name,
                                    Description = authorizationPolicy.Description,
                                    CreatedById = userId,
                                    CreatedDate = DateTime.UtcNow,
                                    UpdatedById = userId,
                                    UpdatedDate = DateTime.UtcNow
                                }, authorizationPolicyTableName);

                                var roleAuthorizationPolicyTableName = nameof(RoleAuthorizationPolicy);
                                await dataConnection.InsertAsync(new RoleAuthorizationPolicy()
                                {
                                    GrantedById = userId,
                                    GrantedDate = DateTime.UtcNow,
                                    IsGranted = true,
                                    RoleId = adminRoleId,
                                    AuthorizationPolicyId = authorizationPolicyId
                                }, roleAuthorizationPolicyTableName);

                                var userAuthorizationPolicyTableName = nameof(UserAuthorizationPolicy);
                                await dataConnection.InsertAsync(new UserAuthorizationPolicy()
                                {
                                    GrantedById = userId,
                                    GrantedDate = DateTime.UtcNow,
                                    IsGranted = true,
                                    UserId = userId,
                                    AuthorizationPolicyId = authorizationPolicyId
                                }, userAuthorizationPolicyTableName);
                            }

                            await transaction.CommitAsync();
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
        }

        public async Task PrepareContentDataAsync(SetupRequest installationRequest)
        {
            using (var dataConnection = _contentDataProvider.CreateDataConnection())
            {
                using (var transaction = dataConnection.BeginTransaction())
                {

                    try
                    {
                        // Insert countries
                        var userPhotoTypeTableName = nameof(UserPhotoType);
                        foreach (var userPhotoType in installationRequest.UserPhotoTypes)
                        {
                            await dataConnection.InsertAsync(new UserPhotoType()
                            {
                                Name = userPhotoType.Name,
                                Description = userPhotoType.Description
                            }, userPhotoTypeTableName);
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
        }
    }
}
