using Camino.Business.Contracts;
using Camino.Business.AutoMap;
using Camino.Business.ValidationStrategies;
using Camino.Business.Dtos.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Data.Entities.Identity;
using System.Collections.Generic;
using Camino.Data.Contracts;
using AutoMapper;
using Camino.IdentityDAL.Contracts;

namespace Camino.Business.Implementation.UserBusiness
{
    public partial class AuthenticationBusiness : IAuthenticationBusiness
    {
        #region Fields/Properties
        private readonly IRepository<UserInfo> _userInfoRepository;
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
            IRepository<UserInfo> userInfoRepository,
            IRepository<UserRole> userRoleRepository,
            IIdentityDataProvider identityDbProvider)
        {
            _identityDbProvider = identityDbProvider;
            _mapper = mapper;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _userRoleRepository = userRoleRepository;
            _validationStrategyContext = validationStrategyContext;
        }
        #endregion

        #region CRUD
        
        public async Task<UserDto> UpdatePasswordAsync(UserPasswordUpdateDto model)
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
                return new UserDto();
            }
            user.PasswordHash = model.NewPassword;
            await _userRepository.UpdateAsync(user);
            return new UserDto()
            {
                Id = user.Id
            };
        }
        #endregion

        #region GET
        public IEnumerable<UserRoleDto> GetUserRoles(long userd)
        {
            var userRoles = (from user in _userRepository.Table
                             join userRole in _userRoleRepository.Table
                             on user.Id equals userRole.UserId into roles
                             from userRole in roles.DefaultIfEmpty()
                             where user.Id == userd
                             select new UserRoleDto()
                             {
                                 UserId = user.Id,
                                 RoleId = userRole.RoleId,
                                 RoleName = userRole.Role.Name
                             }).ToList();

            return userRoles;
        }

        public UserDto GetLoggedIn(long id)
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
