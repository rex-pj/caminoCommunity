using Camino.Application.Contracts.AppServices.Setup;
using Camino.Application.Contracts.AppServices.Setup.Dtos;
using Camino.Core.Contracts.Repositories.Setup;
using Camino.Core.Domains.Authorization;
using Camino.Core.DependencyInjection;
using Camino.Core.Domains.Identifiers;
using Camino.Core.Domains.Media;
using Camino.Core.Domains.Navigations;
using Camino.Core.Domains.Users;
using Camino.Shared.Enums;
using Camino.Core.Domains;
using Camino.Infrastructure.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace Camino.Application.AppServices.Setup
{
    public class DataSeedAppService : IDataSeedAppService, IScopedDependency
    {
        private readonly IDbCreationRepository _dbCreationRepository;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<Status> _userStatusRepository;
        private readonly IEntityRepository<UserPhotoType> _userPhotoTypeRepository;
        private readonly IEntityRepository<Shortcut> _shortcutRepository;
        private readonly IEntityRepository<Gender> _genderRepository;
        private readonly IEntityRepository<Country> _countryRepository;
        private readonly IEntityRepository<Role> _roleRepository;
        private readonly IEntityRepository<UserRole> _userRoleRepository;
        private readonly IEntityRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IEntityRepository<RoleAuthorizationPolicy> _roleAuthorizationPolicyRepository;
        private readonly IEntityRepository<UserAuthorizationPolicy> _userAuthorizationPolicyRepository;
        private readonly IAppDbContext _dbContext;
        
        public DataSeedAppService(IDbCreationRepository dbCreationRepository,
            IAppDbContext dbContext, IEntityRepository<User> userRepository,
            IEntityRepository<Status> userStatusRepository,
            IEntityRepository<UserPhotoType> userPhotoTypeRepository,
            IEntityRepository<Shortcut> shortcutRepository,
            IEntityRepository<Gender> genderRepository,
            IEntityRepository<Country> countryRepository,
            IEntityRepository<Role> roleRepository,
            IEntityRepository<UserRole> userRoleRepository,
            IEntityRepository<AuthorizationPolicy> authorizationPolicyRepository,
            IEntityRepository<RoleAuthorizationPolicy> roleAuthorizationPolicyRepository,
            IEntityRepository<UserAuthorizationPolicy> userAuthorizationPolicyRepository)
        {
            _dbCreationRepository = dbCreationRepository;
            _userRepository = userRepository;
            _userStatusRepository = userStatusRepository;
            _userPhotoTypeRepository = userPhotoTypeRepository;
            _shortcutRepository = shortcutRepository;
            _genderRepository = genderRepository;
            _countryRepository = countryRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _authorizationPolicyRepository = authorizationPolicyRepository;
            _roleAuthorizationPolicyRepository = roleAuthorizationPolicyRepository;
            _userAuthorizationPolicyRepository = userAuthorizationPolicyRepository;
            _dbContext = dbContext;
        }

        public async Task CreateDatabaseAsync()
        {
            try
            {
                await _dbCreationRepository.CreateDatabaseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
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
            }
            catch (Exception ex)
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
                await _userPhotoTypeRepository.InsertAsync(new UserPhotoType()
                {
                    Name = userPhotoType.Name,
                    Description = userPhotoType.Description
                });
            }

            // Insert Shortcuts
            foreach (var shortcut in setupRequest.Shortcuts)
            {
                await _shortcutRepository.InsertAsync(new Shortcut
                {
                    Name = shortcut.Name,
                    Description = shortcut.Description,
                    Icon = shortcut.Icon,
                    TypeId = shortcut.TypeId,
                    Url = shortcut.Url
                });
            }

            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> SeedIdentityDataAsync(SetupRequest setupRequest)
        {
            await _dbContext.Database.BeginTransactionAsync();
            var modifiedDate = DateTime.UtcNow;
            // Insert user statuses
            int activedStatusId = 0;
            foreach (var statusRequest in setupRequest.Statuses)
            {
                var status = new Status
                {
                    Name = statusRequest.Name,
                    Description = statusRequest.Description
                };

                if (status.Name == UserStatuses.Actived.ToString())
                {
                    await _userStatusRepository.InsertAsync(status);
                    await _dbContext.SaveChangesAsync();
                    activedStatusId = status.Id;
                }
                else
                {
                    await _userStatusRepository.InsertAsync(status);
                }
            }

            if (activedStatusId == 0)
            {
                return false;
            }

            // Insert genders
            foreach (var gender in setupRequest.Genders)
            {
                await _genderRepository.InsertAsync(new Gender()
                {
                    Name = gender.Name
                });
            }

            // Insert countries
            foreach (var country in setupRequest.Countries)
            {
                await _countryRepository.InsertAsync(new Country()
                {
                    Name = country.Name,
                    Code = country.Code
                });
            }

            // Insert user
            var userRequest = setupRequest.InitualUser;
            var user = new User
            {
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Email = userRequest.Email,
                Firstname = userRequest.Firstname,
                Lastname = userRequest.Lastname,
                PasswordHash = userRequest.PasswordHash,
                SecurityStamp = userRequest.SecurityStamp,
                StatusId = activedStatusId,
                UserName = userRequest.UserName,
                DisplayName = userRequest.DisplayName,
                IsEmailConfirmed = true,
                BirthDate = userRequest.BirthDate,
                Description = userRequest.Description,
                GenderId = userRequest.GenderId,
                CountryId = userRequest.CountryId,
                Address = userRequest.Address
            };

            _userRepository.Insert(user);
            _dbContext.SaveChanges();
            var userId = user.Id;
            user.CreatedById = userId;
            user.UpdatedById = userId;
            if (userId > 0)
            {
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
                        CreatedDate = modifiedDate,
                        UpdatedDate = modifiedDate
                    };

                    if (role.Name == "Admin")
                    {
                        await _roleRepository.InsertAsync(newRole);
                        await _dbContext.SaveChangesAsync();
                        adminRoleId = newRole.Id;
                    }
                    else
                    {
                        await _roleRepository.InsertAsync(newRole);
                    }
                }

                if (adminRoleId > 0)
                {
                    // Insert user role
                    var adminUserRole = new UserRole()
                    {
                        UserId = userId,
                        GrantedById = userId,
                        GrantedDate = modifiedDate,
                        IsGranted = true,
                        RoleId = adminRoleId
                    };

                    await _userRoleRepository.InsertAsync(adminUserRole);
                }

                // Insert authorization policies
                foreach (var authorizationPolicy in setupRequest.AuthorizationPolicies)
                {
                    var newAuthorizationPolicy = new AuthorizationPolicy()
                    {
                        Name = authorizationPolicy.Name,
                        Description = authorizationPolicy.Description,
                        CreatedById = userId,
                        CreatedDate = modifiedDate,
                        UpdatedById = userId,
                        UpdatedDate = modifiedDate
                    };
                    await _authorizationPolicyRepository.InsertAsync(newAuthorizationPolicy);
                    await _dbContext.SaveChangesAsync();
                    var authorizationPolicyId = newAuthorizationPolicy.Id;

                    await _roleAuthorizationPolicyRepository.InsertAsync(new RoleAuthorizationPolicy()
                    {
                        GrantedById = userId,
                        GrantedDate = modifiedDate,
                        IsGranted = true,
                        RoleId = adminRoleId,
                        AuthorizationPolicyId = authorizationPolicyId
                    });

                    await _userAuthorizationPolicyRepository.InsertAsync(new UserAuthorizationPolicy()
                    {
                        GrantedById = userId,
                        GrantedDate = modifiedDate,
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
