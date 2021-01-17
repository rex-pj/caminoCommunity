using AutoMapper;
using Camino.DAL.Contracts;
using Camino.IdentityDAL.Contracts;
using LinqToDB;
using System;
using System.Threading.Tasks;
using Camino.Data.Enums;
using Camino.Service.Business.Setup.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.Request;

namespace Camino.Service.Business.Setup
{
    public class IdentityDataSetupBusiness : IIdentityDataSetupBusiness
    {
        private readonly IIdentityDataProvider _identityDataProvider;
        private readonly IMapper _mapper;
        private readonly ISeedDataBusiness _seedDataBusiness;
        public IdentityDataSetupBusiness(IIdentityDataProvider identityDataProvider, IMapper mapper, ISeedDataBusiness seedDataBusiness)
        {
            _identityDataProvider = identityDataProvider;
            _seedDataBusiness = seedDataBusiness;
            _mapper = mapper;
        }

        public bool IsIdentityDatabaseExist()
        {
            return _identityDataProvider.IsDatabaseExist();
        }

        public void SeedingIdentityDb(string sql)
        {
            if (IsIdentityDatabaseExist())
            {
                return;
            }

            _seedDataBusiness.CreateDatabase(_identityDataProvider);
            _seedDataBusiness.CreateDataByScript(_identityDataProvider, sql);
        }

        public async Task PrepareIdentityDataAsync(SetupRequest installationRequest)
        {
            using (var dataConnection = _identityDataProvider.CreateDataConnection())
            {
                using (var transaction = await dataConnection.BeginTransactionAsync())
                {
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
                            activedStatusId = await dataConnection.InsertWithInt32IdentityAsync(status);
                        }
                        else
                        {
                            await dataConnection.InsertAsync(status);
                        }
                    }

                    if (activedStatusId == 0)
                    {
                        return;
                    }

                    // Insert genders
                    foreach (var gender in installationRequest.Genders)
                    {
                        await dataConnection.InsertAsync(new Gender()
                        {
                            Name = gender.Name
                        });
                    }

                    // Insert countries
                    foreach (var country in installationRequest.Countries)
                    {
                        await dataConnection.InsertAsync(new Country()
                        {
                            Name = country.Name,
                            Code = country.Code
                        });
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
                        var userId = await dataConnection.InsertWithInt64IdentityAsync(user);
                        if (userId > 0)
                        {
                            userInfo.Id = userId;
                            await dataConnection.InsertWithInt64IdentityAsync(userInfo);

                            // Insert roles
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
                                    adminRoleId = await dataConnection.InsertAsync(newRole);
                                }
                                else
                                {
                                    await dataConnection.InsertAsync(newRole);
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

                                await dataConnection.InsertAsync(adminUserRole);
                            }

                            // Insert authorization policies
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
                                });

                                await dataConnection.InsertAsync(new RoleAuthorizationPolicy()
                                {
                                    GrantedById = userId,
                                    GrantedDate = DateTime.UtcNow,
                                    IsGranted = true,
                                    RoleId = adminRoleId,
                                    AuthorizationPolicyId = authorizationPolicyId
                                });

                                await dataConnection.InsertAsync(new UserAuthorizationPolicy()
                                {
                                    GrantedById = userId,
                                    GrantedDate = DateTime.UtcNow,
                                    IsGranted = true,
                                    UserId = userId,
                                    AuthorizationPolicyId = authorizationPolicyId
                                });
                            }

                            await transaction.CommitAsync();
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                        }
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
        }

        
    }
}
