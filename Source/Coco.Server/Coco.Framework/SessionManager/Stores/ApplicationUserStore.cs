using AutoMapper;
using Coco.Business.Contracts;
using Coco.Common.Const;
using Coco.Common.Resources;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using Coco.Framework.Models;
using Coco.Framework.Services.Contracts;
using Coco.Framework.SessionManager.Core;
using Coco.Framework.SessionManager.Stores.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
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
        IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserAuthenticationTokenStore<ApplicationUser>
    {
        private readonly IMapper _mapper;
        private readonly string _appName;
        private readonly string _registerConfirmUrl;
        private readonly string _resetPasswordUrl;
        private readonly string _registerConfirmFromEmail;
        private readonly string _registerConfirmFromName;
        private readonly IEmailSender _emailSender;
        private readonly IUserBusiness _userBusiness;
        private readonly IUserAttributeStore<ApplicationUser> _userAttributeStore;
        private readonly IUserClaimBusiness _userClaimBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IUserTokenBusiness _userTokenBusiness;

        public override IQueryable<ApplicationUser> Users => throw new NotImplementedException();

        public ApplicationUserStore(IdentityErrorDescriber describer, IUserBusiness userBusiness, 
            IUserAttributeStore<ApplicationUser> userAttributeStore, IUserClaimBusiness userClaimBusiness,
            IUserRoleBusiness userRoleBusiness, IRoleBusiness roleBusiness, IUserTokenBusiness userTokenBusiness,
            IMapper mapper, IConfiguration configuration, IEmailSender emailSender) 
            : base(describer)
        {
            _userBusiness = userBusiness;
            _userAttributeStore = userAttributeStore;
            _userClaimBusiness = userClaimBusiness;
            _userRoleBusiness = userRoleBusiness;
            _userTokenBusiness = userTokenBusiness;
            _roleBusiness = roleBusiness;
            _mapper = mapper;
            _emailSender = emailSender;

            _appName = configuration[ConfigurationSettingsConst.APPLICATION_NAME];
            _registerConfirmUrl = configuration[ConfigurationSettingsConst.REGISTER_CONFIRMATION_URL];
            _registerConfirmFromEmail = configuration[ConfigurationSettingsConst.REGISTER_CONFIRM_FROM_EMAIL];
            _registerConfirmFromName = configuration[ConfigurationSettingsConst.REGISTER_CONFIRM_FROM_NAME];
            _resetPasswordUrl = configuration[ConfigurationSettingsConst.RESET_PASSWORD_URL];
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
                    userResponse.ActiveUserStamp = user.ActiveUserStamp;
                    userResponse.SecurityStamp = user.SecurityStamp;

                    var newUserAttributes = _userAttributeStore.NewUserRegisterAttributes(userResponse);
                    await _userAttributeStore.SetAttributesAsync(newUserAttributes);

                    // Send activation email
                    await SendActiveEmailAsync(userResponse, cancellationToken);
                }

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> SendActiveEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                var activeUserUrl = $"{_registerConfirmUrl}/{user.Email}/{user.ActiveUserStamp}";
                await _emailSender.SendEmailAsync(new MailMessageModel()
                {
                    Body = string.Format(MailTemplateResources.USER_CONFIRMATION_BODY, user.DisplayName, _appName, activeUserUrl),
                    FromEmail = _registerConfirmFromEmail,
                    FromName = _registerConfirmFromName,
                    ToEmail = user.Email,
                    ToName = user.DisplayName,
                    Subject = string.Format(MailTemplateResources.USER_CONFIRMATION_SUBJECT, _appName),
                }, TextFormat.Html);

                return IdentityResult.Success;
            }
            catch (Exception ex)
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
            return _mapper.Map<ApplicationUser>(user);
        }

        public override async Task<string> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var securityStamp = await _userAttributeStore.GetSecurityStampAsync(user.Id, UserAttributeOptions.SECURITY_SALT);
            user.SecurityStamp = securityStamp;
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
            throw new NotImplementedException();
        }

        protected override Task<ApplicationUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<ApplicationUserLogin> FindUserLoginAsync(long userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
    }
}