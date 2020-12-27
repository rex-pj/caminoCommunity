using Camino.Service.AutoMap;
using Camino.Service.Strategies.Validation;
using Camino.Service.Projections.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Camino.Data.Contracts;
using AutoMapper;
using Camino.IdentityDAL.Contracts;
using Camino.Service.Business.Authentication.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.Request;

namespace Camino.Service.Business.Authentication
{
    public partial class AuthenticationBusiness : IAuthenticationBusiness
    {
        #region Fields/Properties
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly ValidationStrategyContext _validationStrategyContext;
        private readonly IMapper _mapper;
        private readonly IIdentityDataProvider _identityDbProvider;
        #endregion

        #region Ctor
        public AuthenticationBusiness(IRepository<User> userRepository,
            ValidationStrategyContext validationStrategyContext,
            IMapper mapper,
            IRepository<UserRole> userRoleRepository,
            IIdentityDataProvider identityDbProvider)
        {
            _identityDbProvider = identityDbProvider;
            _mapper = mapper;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _validationStrategyContext = validationStrategyContext;
        }
        #endregion

        #region CRUD
        
        public async Task<UserProjection> UpdatePasswordAsync(UserPasswordUpdateRequest model)
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

            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if(user == null)
            {
                return new UserProjection();
            }
            user.PasswordHash = model.NewPassword;
            await _userRepository.UpdateAsync(user);
            return new UserProjection()
            {
                Id = user.Id
            };
        }
        #endregion

        #region GET
        public IEnumerable<UserRoleProjection> GetUserRoles(long userd)
        {
            var userRoles = (from user in _userRepository.Table
                             join userRole in _userRoleRepository.Table
                             on user.Id equals userRole.UserId into roles
                             from userRole in roles.DefaultIfEmpty()
                             where user.Id == userd
                             select new UserRoleProjection()
                             {
                                 UserId = user.Id,
                                 RoleId = userRole.RoleId,
                                 RoleName = userRole.Role.Name
                             }).ToList();

            return userRoles;
        }

        public UserProjection GetLoggedIn(long id)
        {
            var user = _userRepository
                .Get(x => x.Id == id)
                .Select(UserExpressionMapping.UserModelSelector)
                .FirstOrDefault();

            return user;
        }
        #endregion
    }
}
