using System;
using System.Threading.Tasks;
using Camino.Shared.Enums;
using Camino.Core.Contracts.Repositories.Setup;
using Camino.Core.Domain.Identifiers;
using Camino.Core.Domain.Media;
using Camino.Shared.Requests.Setup;
using Camino.Core.Domain.Navigations;
using Camino.Core.Contracts.DependencyInjection;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Setup
{
    public class DataSeedRepository : IDataSeedRepository, IScopedDependency
    {
        private readonly CaminoDbContext _dbContext;

        public DataSeedRepository(CaminoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedDataAsync(SetupRequest setupRequest)
        {
            try
            {
                var isSucceed = await SeedIdentityDataAsync(setupRequest);
                if (isSucceed)
                {
                    await SeedContentDataAsync(setupRequest);
                    await _dbContext.Database.CommitTransactionAsync();
                }
                else
                {
                    await _dbContext.Database.RollbackTransactionAsync();
                }
            }
            catch (Exception)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task SeedContentDataAsync(SetupRequest setupRequest)
        {
            // Insert User Photo Types
            foreach (var userPhotoType in setupRequest.UserPhotoTypes)
            {
                await _dbContext.UserPhotoTypes.AddAsync(new UserPhotoType()
                {
                    Name = userPhotoType.Name,
                    Description = userPhotoType.Description
                });
            }

            // Insert Shortcuts
            foreach (var shortcut in setupRequest.Shortcuts)
            {
                await _dbContext.Shortcuts.AddAsync(new Shortcut
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
                    await _dbContext.Statuses.AddAsync(status);
                    await _dbContext.SaveChangesAsync();
                    activedStatusId = status.Id;
                }
                else
                {
                    await _dbContext.Statuses.AddAsync(status);
                }
            }

            if (activedStatusId == 0)
            {
                return false;
            }

            // Insert genders
            foreach (var gender in setupRequest.Genders)
            {
                await _dbContext.Genders.AddAsync(new Gender()
                {
                    Name = gender.Name
                });
            }

            // Insert countries
            foreach (var country in setupRequest.Countries)
            {
                await _dbContext.Countries.AddAsync(new Country()
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

            long userId = 0;
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            userId = user.Id;
            if (userId > 0)
            {
                await _dbContext.UserInfos.AddAsync(new UserInfo
                {
                    BirthDate = userRequest.BirthDate,
                    Description = userRequest.Description,
                    GenderId = userRequest.GenderId,
                    Id = userId
                });

                // Insert roles
                long adminRoleId = 0;
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
                        await _dbContext.Roles.AddAsync(newRole);
                        await _dbContext.SaveChangesAsync();
                        adminRoleId = newRole.Id;
                    }
                    else
                    {
                        await _dbContext.Roles.AddAsync(newRole);
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

                    await _dbContext.UserRoles.AddAsync(adminUserRole);
                }

                // Insert authorization policies
                foreach (var authorizationPolicy in setupRequest.AuthorizationPolicies)
                {
                    var newAuthorizationPolicy = new AuthorizationPolicy()
                    {
                        Name = authorizationPolicy.Name,
                        Description = authorizationPolicy.Description,
                        CreatedById = userId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedById = userId,
                        UpdatedDate = DateTime.UtcNow
                    };
                    await _dbContext.AuthorizationPolicies.AddAsync(newAuthorizationPolicy);
                    await _dbContext.SaveChangesAsync();
                    var authorizationPolicyId = newAuthorizationPolicy.Id;

                    await _dbContext.RoleAuthorizationPolicies.AddAsync(new RoleAuthorizationPolicy()
                    {
                        GrantedById = userId,
                        GrantedDate = DateTime.UtcNow,
                        IsGranted = true,
                        RoleId = adminRoleId,
                        AuthorizationPolicyId = authorizationPolicyId
                    });

                    await _dbContext.UserAuthorizationPolicies.AddAsync(new UserAuthorizationPolicy()
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
