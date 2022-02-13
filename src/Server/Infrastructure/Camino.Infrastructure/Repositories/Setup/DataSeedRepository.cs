using LinqToDB;
using System;
using System.Threading.Tasks;
using Camino.Shared.Enums;
using Camino.Core.Contracts.Repositories.Setup;
using Camino.Core.Domain.Identifiers;
using Camino.Core.Domain.Media;
using Camino.Shared.Requests.Setup;
using Camino.Core.Domain.Navigations;
using Camino.Infrastructure.Linq2Db;

namespace Camino.Infrastructure.Repositories.Setup
{
    public class DataSeedRepository : IDataSeedRepository
    {
        private readonly CaminoDataConnection _caminoDataConnection;

        public DataSeedRepository(CaminoDataConnection caminoDataConnection)
        {
            _caminoDataConnection = caminoDataConnection;
        }

        public async Task SeedDataAsync(SetupRequest setupRequest)
        {
            try
            {
                var isSucceed = await SeedIdentityDataAsync(setupRequest);
                if (isSucceed)
                {
                    await SeedContentDataAsync(setupRequest);
                    await _caminoDataConnection.CommitTransactionAsync();
                }
                else
                {
                    await _caminoDataConnection.RollbackTransactionAsync();
                }
            }
            catch (Exception ex)
            {
                await _caminoDataConnection.RollbackTransactionAsync();
                throw new LinqToDBException(ex);
            }
        }

        private async Task SeedContentDataAsync(SetupRequest setupRequest)
        {
            // Insert User Photo Types
            foreach (var userPhotoType in setupRequest.UserPhotoTypes)
            {
                await _caminoDataConnection.InsertAsync(new UserPhotoType()
                {
                    Name = userPhotoType.Name,
                    Description = userPhotoType.Description
                });
            }

            // Insert Shortcuts
            foreach (var shortcut in setupRequest.Shortcuts)
            {
                await _caminoDataConnection.InsertAsync(new Shortcut
                {
                    Name = shortcut.Name,
                    Description = shortcut.Description,
                    Icon = shortcut.Icon,
                    TypeId = shortcut.TypeId,
                    Url = shortcut.Url
                });
            }
        }

        private async Task<bool> SeedIdentityDataAsync(SetupRequest setupRequest)
        {
            // Insert user statuses
            int activedStatusId = 0;
            foreach (var statusRequest in setupRequest.Statuses)
            {
                var status = new Status
                {
                    Name = statusRequest.Name,
                    Description = statusRequest.Description
                };

                if (status.Name == UserStatus.Actived.ToString())
                {
                    activedStatusId = await _caminoDataConnection.InsertWithInt32IdentityAsync(status);
                }
                else
                {
                    await _caminoDataConnection.InsertAsync(status);
                }
            }

            if (activedStatusId == 0)
            {
                return false;
            }

            // Insert genders
            foreach (var gender in setupRequest.Genders)
            {
                await _caminoDataConnection.InsertAsync(new Gender()
                {
                    Name = gender.Name
                });
            }

            // Insert countries
            foreach (var country in setupRequest.Countries)
            {
                await _caminoDataConnection.InsertAsync(new Country()
                {
                    Name = country.Name,
                    Code = country.Code
                });
            }

            // Insert user
            var userRequest = setupRequest.InitualUser;
            var user = new User
            {
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Email = userRequest.Email,
                Firstname = userRequest.Firstname,
                Lastname = userRequest.Lastname,
                PasswordHash = userRequest.PasswordHash,
                SecurityStamp = userRequest.SecurityStamp,
                StatusId = activedStatusId,
                UserName = userRequest.UserName,
                DisplayName = userRequest.DisplayName,
                IsEmailConfirmed = true
            };

            var userId = await _caminoDataConnection.InsertWithInt64IdentityAsync(user);
            if (userId > 0)
            {
                await _caminoDataConnection.InsertWithInt64IdentityAsync(new UserInfo
                {
                    BirthDate = userRequest.BirthDate,
                    Description = userRequest.Description,
                    GenderId = userRequest.GenderId,
                    Id = userId
                });

                // Insert roles
                var adminRoleId = 0;
                foreach (var role in setupRequest.Roles)
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
                        adminRoleId = await _caminoDataConnection.InsertAsync(newRole);
                    }
                    else
                    {
                        await _caminoDataConnection.InsertAsync(newRole);
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

                    await _caminoDataConnection.InsertAsync(adminUserRole);
                }

                // Insert authorization policies
                foreach (var authorizationPolicy in setupRequest.AuthorizationPolicies)
                {
                    var authorizationPolicyId = await _caminoDataConnection.InsertWithInt64IdentityAsync(new AuthorizationPolicy()
                    {
                        Name = authorizationPolicy.Name,
                        Description = authorizationPolicy.Description,
                        CreatedById = userId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedById = userId,
                        UpdatedDate = DateTime.UtcNow
                    });

                    await _caminoDataConnection.InsertAsync(new RoleAuthorizationPolicy()
                    {
                        GrantedById = userId,
                        GrantedDate = DateTime.UtcNow,
                        IsGranted = true,
                        RoleId = adminRoleId,
                        AuthorizationPolicyId = authorizationPolicyId
                    });

                    await _caminoDataConnection.InsertAsync(new UserAuthorizationPolicy()
                    {
                        GrantedById = userId,
                        GrantedDate = DateTime.UtcNow,
                        IsGranted = true,
                        UserId = userId,
                        AuthorizationPolicyId = authorizationPolicyId
                    });
                }

                return true;
            }

            return false;
        }
    }
}
