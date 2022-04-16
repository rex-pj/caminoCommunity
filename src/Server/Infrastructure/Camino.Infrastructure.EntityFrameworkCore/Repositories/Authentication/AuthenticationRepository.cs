using Camino.Shared.Results.Identifiers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Core.Contracts.Data;
using Camino.Core.Contracts.Repositories.Authentication;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Requests.Authentication;
using Camino.Shared.Results.Authorization;
using Camino.Core.Contracts.DependencyInjection;
using Camino.Core.Validations;
using Camino.Core.Contracts.Validations;

namespace Camino.Infrastructure.EntityFrameworkCore.Repositories.Authentication
{
    public partial class AuthenticationRepository : IAuthenticationRepository, IScopedDependency
    {
        #region Fields/Properties
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<UserRole> _userRoleRepository;
        private readonly IValidationStrategyContext _validationStrategyContext;
        private readonly IAppDbContext _dbContext;

        #endregion

        #region Ctor
        public AuthenticationRepository(IEntityRepository<User> userRepository,
            IValidationStrategyContext validationStrategyContext,
            IEntityRepository<UserRole> userRoleRepository, IAppDbContext dbContext)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _validationStrategyContext = validationStrategyContext;
            _dbContext = dbContext;
        }
        #endregion

        #region CRUD
        
        public async Task<UserResult> UpdatePasswordAsync(UserPasswordUpdateRequest model)
        {
            _validationStrategyContext.SetStrategy(new UserPasswordUpdateValidationStratergy());
            bool canUpdate = _validationStrategyContext.Validate(model);
            if (!canUpdate)
            {
                throw new UnauthorizedAccessException();
            }

            var errors = _validationStrategyContext.Errors;
            if (errors != null || errors.Any())
            {
                throw new ArgumentException(_validationStrategyContext.Errors.FirstOrDefault().Message);
            }

            var user = await _userRepository.FindAsync(x => x.Id == model.UserId);
            if(user == null)
            {
                return new UserResult();
            }
            user.PasswordHash = model.NewPassword;
            await _userRepository.UpdateAsync(user);
            await _dbContext.SaveChangesAsync();
            return new UserResult()
            {
                Id = user.Id
            };
        }
        #endregion

        #region GET
        public IEnumerable<UserRoleResult> GetUserRoles(long userd)
        {
            var userRoles = (from user in _userRepository.Table
                             join userRole in _userRoleRepository.Table
                             on user.Id equals userRole.UserId into roles
                             from userRole in roles.DefaultIfEmpty()
                             where user.Id == userd
                             select new UserRoleResult()
                             {
                                 UserId = user.Id,
                                 RoleId = userRole.RoleId,
                                 RoleName = userRole.Role.Name
                             }).ToList();

            return userRoles;
        }
        #endregion
    }
}
