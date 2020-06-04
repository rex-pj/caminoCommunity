using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.SessionManager.Stores.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Stores
{
    public class ApplicationUserStore : UserStoreBase<ApplicationUser, ApplicationRole, long, ApplicationUserClaim, 
        ApplicationUserRole, ApplicationUserLogin, ApplicationUserToken, ApplicationRoleClaim>, 
        IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserAuthenticationTokenStore<ApplicationUser>,
        IUserEncryptionStore<ApplicationUser>
    {
        private readonly IMapper _mapper;
        
        private readonly IUserBusiness _userBusiness;
        private readonly IUserAttributeStore<ApplicationUser> _userAttributeStore;
        private readonly IUserClaimBusiness _userClaimBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IUserTokenBusiness _userTokenBusiness;
        private readonly IUserLoginBusiness _userLoginBusiness;
        private readonly ITextEncryption _textCrypter;
        private readonly string _textCrypterSaltKey;

        public override IQueryable<ApplicationUser> Users { get; }

        public ApplicationUserStore(IdentityErrorDescriber describer, IUserBusiness userBusiness, 
            IUserAttributeStore<ApplicationUser> userAttributeStore, IUserClaimBusiness userClaimBusiness,
            IUserRoleBusiness userRoleBusiness, IRoleBusiness roleBusiness, IUserTokenBusiness userTokenBusiness,
            IUserLoginBusiness userLoginBusiness,
            ITextEncryption textCrypter, IMapper mapper, IConfiguration configuration) 
            : base(describer)
        {
            _textCrypterSaltKey = configuration["Crypter:SaltKey"];
            _userBusiness = userBusiness;
            _userAttributeStore = userAttributeStore;
            _userClaimBusiness = userClaimBusiness;
            _userRoleBusiness = userRoleBusiness;
            _userTokenBusiness = userTokenBusiness;
            _roleBusiness = roleBusiness;
            _userLoginBusiness = userLoginBusiness;
            _mapper = mapper;
            _textCrypter = textCrypter;
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                var userDto = _mapper.Map<UserDto>(user);
                userDto.PasswordHash = user.PasswordHash;

                var response = await _userBusiness.CreateAsync(userDto);
                if (response.Id > 0)
                {
                    var userResponse = _mapper.Map<ApplicationUser>(response);
                    userResponse.SecurityStamp = user.SecurityStamp;

                    var newUserAttributes = _userAttributeStore.NewUserRegisterAttributes(userResponse);
                    await _userAttributeStore.SetAttributesAsync(newUserAttributes);
                }

                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }
        }

        public override async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var id = long.Parse(userId);
            var user = await _userBusiness.FindByIdAsync(id);
            return _mapper.Map<ApplicationUser>(user);
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                await _userBusiness.DeleteAsync(user.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public override async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = await _userBusiness.FindByUsernameAsync(normalizedUserName);
            if (user != null)
            {
                user.SecurityStamp = await _userAttributeStore.GetSecurityStampAsync(user.Id);
            }
            
            return _mapper.Map<ApplicationUser>(user);
        }

        public override async Task<string> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            return await base.GetSecurityStampAsync(user, cancellationToken);
        }

        public override async Task AddToRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(nameof(normalizedRoleName));
            }
            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (roleEntity == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Role Not Found", normalizedRoleName));
            }
            var applicationUserRole = CreateUserRole(user, roleEntity);
            var userRoleDto = _mapper.Map<UserRoleDto>(applicationUserRole);
            _userRoleBusiness.Add(userRoleDto);
        }

        protected override async Task<ApplicationRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await _roleBusiness.FindByNameAsync(normalizedRoleName);
            return _mapper.Map<ApplicationRole>(role);
        }

        protected override async Task<ApplicationUserRole> FindUserRoleAsync(long userId, long roleId, CancellationToken cancellationToken)
        {
            var userRole = await _userRoleBusiness.FindUserRoleAsync(userId, roleId);
            return _mapper.Map<ApplicationUserRole>(userRole);
        }

        public override async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var roles = (await _userRoleBusiness.GetUserRolesAsync(user.Id))
                .Select(x => x.RoleName).ToList();
            
            return roles;
        }

        public override async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(normalizedRoleName))
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);

            if (role != null)
            {
                var users = await _userRoleBusiness.GetUsersInRoleAsync(role.Id);
                return _mapper.Map<IList<ApplicationUser>>(users);
            }
            return new List<ApplicationUser>();
        }

        public override async Task<bool> IsInRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(nameof(normalizedRoleName));
            }
            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
                return userRole != null;
            }
            return false;
        }

        public override async Task RemoveFromRoleAsync(ApplicationUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(nameof(normalizedRoleName));
            }

            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (roleEntity != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);
                if (userRole != null)
                {
                    var userRoleDto = _mapper.Map<UserRoleDto>(userRole);
                    _userRoleBusiness.Remove(userRoleDto);
                }
            }
        }

        public override Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            foreach (var claim in claims)
            {
                var userClaim = _mapper.Map<UserClaimDto>(CreateUserClaim(user, claim));
                _userClaimBusiness.Add(userClaim);
            }
            return Task.FromResult(false);
        }

        protected override ApplicationUserClaim CreateUserClaim(ApplicationUser user, Claim claim)
        {
            var userClaim = new ApplicationUserClaim { UserId = user.Id };
            userClaim.InitializeFromClaim(claim);
            return userClaim;
        }

        public override async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            var user = await _userBusiness.FindByEmailAsync(normalizedEmail);
            return _mapper.Map<ApplicationUser>(user);
        }

        protected override async Task<ApplicationUser> FindUserAsync(long userId, CancellationToken cancellationToken)
        {
            var user = await _userBusiness.FindByIdAsync(userId);
            return _mapper.Map<ApplicationUser>(user);
        }

        public override Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var userLogin = CreateUserLogin(user, login);
            var userLoginDto = _mapper.Map<UserLoginDto>(userLogin);
            _userLoginBusiness.Add(userLoginDto);
            return Task.FromResult(false);
        }

        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userLoginDto = await _userLoginBusiness.FindAsync(loginProvider, providerKey);
            return _mapper.Map<ApplicationUserLogin>(userLoginDto);
        }

        protected override async Task<ApplicationUserLogin> FindUserLoginAsync(long userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userLoginDto = await _userLoginBusiness.FindAsync(userId, loginProvider, providerKey);
            return _mapper.Map<ApplicationUserLogin>(userLoginDto);
        }

        public override async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var userInfos = (await _userLoginBusiness.GetByUserIdAsync(user.Id))
                .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName))
                .ToList();

            return userInfos;
        }

        public override async Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var entry = await FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
            if (entry != null)
            {
                var userLogin = _mapper.Map<UserLoginDto>(entry);
                _userLoginBusiness.Remove(userLogin);
            }
        }

        public override async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userClaims = await _userClaimBusiness.GetByUserIdAsync(user.Id);
            var claims = _mapper.Map<IList<ApplicationUserClaim>>(userClaims);
            return claims.Select(c => c.ToClaim()).ToList();
        }

        public override async Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var claimDto = _mapper.Map<ClaimDto>(claim);
            var userClaims = await _userClaimBusiness.GetUsersForClaimAsync(claimDto);
            var applicationUserClaims = _mapper.Map<IList<ApplicationUser>>(userClaims);

            return applicationUserClaims;
        }

        public override async Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            foreach (var claim in claims)
            {
                var userClaims = await _userClaimBusiness.GetByClaimAsync(user.Id, claim.Value, claim.Type);
                foreach (var c in userClaims)
                {
                    _userClaimBusiness.Remove(c);
                }
            }
        }

        protected override Task AddUserTokenAsync(ApplicationUserToken token)
        {
            var userToken = _mapper.Map<UserTokenDto>(token);
            _userTokenBusiness.Add(userToken);
            return Task.CompletedTask;
        }

        protected override async Task<ApplicationUserToken> FindTokenAsync(ApplicationUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            var userToken = await _userTokenBusiness.FindAsync(user.Id, loginProvider, name);
            return _mapper.Map<ApplicationUserToken>(userToken);
        }

        protected override Task RemoveUserTokenAsync(ApplicationUserToken token)
        {
            var userToken = _mapper.Map<UserTokenDto>(token);
            _userTokenBusiness.Remove(userToken);
            return Task.CompletedTask;
        }

        public override async Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            if (newClaim == null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }

            var claimDto = _mapper.Map<ClaimDto>(claim);
            var newClaimDto = _mapper.Map<ClaimDto>(newClaim);
            await _userClaimBusiness.ReplaceClaimAsync(user.Id, claimDto, newClaimDto);
        }

        public override async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            
            try
            {
                var userDto = _mapper.Map<UserDto>(user);
                await _userBusiness.UpdateAsync(userDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public async Task<string> EncryptUserId(long userId)
        {
            var encryptId = _textCrypter.Encrypt(userId.ToString(), _textCrypterSaltKey);
            return await Task.FromResult(encryptId);
        }

        public async Task<long> DecryptUserId(string userIdentityId)
        {
            var id = long.Parse(_textCrypter.Decrypt(userIdentityId, _textCrypterSaltKey));
            return await Task.FromResult(id);
        }
    }
}