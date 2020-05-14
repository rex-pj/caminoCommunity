using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coco.Framework.Commons.Enums;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Coco.Entities.Dtos.General;
using Coco.Entities.Dtos.User;
using Coco.Entities.Enums;
using Coco.Framework.SessionManager.Core;
using AutoMapper;
using Coco.Commons.Models;
using Coco.Common.Exceptions;
using Coco.Common.Const;

namespace Coco.Framework.SessionManager
{
    public class UserManager : IUserManager<ApplicationUser>
    {
        #region Properties
        public const string ResetPasswordTokenPurpose = "ResetPassword";
        public const string ConfirmEmailTokenPurpose = "EmailConfirmation";
        internal readonly string _tokenEncryptKey;
        internal readonly int _tokenExpiryMinutes;
        private readonly IMapper _mapper;
        protected internal IUserStore<ApplicationUser> UserStore;
        protected internal IUserPhotoStore<ApplicationUser> UserPhotoStore;
        protected internal IUserPasswordStore<ApplicationUser> UserPasswordStore;
        protected internal IUserEmailStore<ApplicationUser> UserEmailStore;
        protected internal IUserStampStore<ApplicationUser> UserStampStore;

        public IList<IUserValidator<ApplicationUser>> UserValidators { get; } = new List<IUserValidator<ApplicationUser>>();
        public IList<IPasswordValidator<ApplicationUser>> PasswordValidators { get; } = new List<IPasswordValidator<ApplicationUser>>();
        public IdentityOptions Options { get; set; }
        public IdentityErrorDescriber Describer { get; private set; }
        #endregion

        #region Fields
        private bool _isDisposed;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ILookupNormalizer _lookupNormalizer;
        #endregion

        #region Ctor
        public UserManager(IUserStore<ApplicationUser> userStore,
            IUserPhotoStore<ApplicationUser> userPhotoStore,
            IUserEmailStore<ApplicationUser> userEmailStore,
            IUserPasswordStore<ApplicationUser> userPasswordStore,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IOptions<IdentityOptions> optionsAccessor,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IConfiguration configuration,
            ILookupNormalizer lookupNormalizer,
            IMapper mapper,
            IUserStampStore<ApplicationUser> userStampStore,
            IdentityErrorDescriber errors = null)
        {
            Options = optionsAccessor?.Value ?? new IdentityOptions();
            UserStore = userStore;
            UserPhotoStore = userPhotoStore;
            UserEmailStore = userEmailStore;
            UserPasswordStore = userPasswordStore;
            UserStampStore = userStampStore;
            _mapper = mapper;

            _passwordHasher = passwordHasher;
            _lookupNormalizer = lookupNormalizer;
            _tokenEncryptKey = configuration["Jwt:SecretKey"];
            int.TryParse(configuration["Jwt:ExpiryMinutes"], out _tokenExpiryMinutes);
            Describer = errors ?? new IdentityErrorDescriber();

            if (userValidators != null)
            {
                foreach (var v in userValidators)
                {
                    this.UserValidators.Add(v);
                }
            }

            if (passwordValidators != null)
            {
                foreach (var v in passwordValidators)
                {
                    this.PasswordValidators.Add(v);
                }
            }
        }
        #endregion

        #region Methods
        protected virtual async Task<bool> UpdatePasswordHash(ApplicationUser user, string newPassword, bool validatePassword = true)
        {
            if (validatePassword)
            {
                var isValidated = await ValidatePasswordAsync(user, newPassword);
                if (!isValidated)
                {
                    return isValidated;
                }
            }

            // Custom password hashing
            string passwordHashed = null;
            if (!string.IsNullOrEmpty(newPassword))
            {
                string passwordSalted = UserPasswordStore.AddSaltToPassword(user, newPassword);
                passwordHashed = _passwordHasher.HashPassword(user, passwordSalted);
            }

            await UserPasswordStore.SetPasswordHashAsync(user, passwordHashed);

            return true;
        }

        protected async Task<bool> ValidatePasswordAsync(ApplicationUser user, string password)
        {
            var errors = new List<CommonError>();
            var isValid = true;
            foreach (var v in PasswordValidators)
            {
                var result = await v.ValidateAsync(this, user, password);
                if (!result.IsSucceed)
                {
                    if (result.Errors.Any())
                    {
                        errors.AddRange(result.Errors);
                    }

                    isValid = false;
                }
            }

            if (!isValid)
            {
                throw new CocoApplicationException(errors);
            }
            return true;
        }

        public virtual async Task<string> GetEmailAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return await UserEmailStore.GetEmailAsync(user);
        }

        public virtual string NormalizeName(string name)
        {
            return (_lookupNormalizer == null) ? name : _lookupNormalizer.NormalizeName(name);
        }

        public virtual string NormalizeEmail(string email)
        {
            return (_lookupNormalizer == null) ? email : _lookupNormalizer.NormalizeEmail(email);
        }

        public virtual string GetUserNameAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return UserStore.GetUserNameAsync(user);
        }

        public virtual async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            email = NormalizeEmail(email);
            var user = await UserStore.FindByEmailAsync(email);
            return user;
        }

        public virtual async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            userName = NormalizeName(userName);

            var user = await UserStore.FindByNameAsync(userName);
            return user;
        }

        public virtual async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return await UserEmailStore.GetEmailConfirmedAsync(user);
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified authenticator token and user id in hashed.
        /// </summary>
        /// <param name="authenticationToken">The authenticator token to search for.</param>
        /// <param name="userIdentityId">The user id hased has been hashed</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="authenticationToken"/> and <paramref name="userIdentityId"/> if it exists.
        /// </returns>
        public virtual ApplicationUser GetLoggingUser(string userIdentityId, string authenticationToken)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(userIdentityId))
            {
                throw new ArgumentNullException(nameof(userIdentityId));
            }

            if (string.IsNullOrEmpty(authenticationToken))
            {
                throw new ArgumentNullException(nameof(authenticationToken));
            }

            var user = UserStore.FindByIdentityId(userIdentityId);
            var tokenValidity = ValidateToken(user.Id, authenticationToken);
            if (tokenValidity.IsSucceed)
            {
                user.AuthenticationToken = tokenValidity.Result.ToString();
            }
            
            return user;
        }

        /// <summary>
        /// Finds and returns a full data of user, if any, who has the specified authenticator token and user id in hashed.
        /// </summary>
        /// <param name="authenticatorToken">The authenticator token to search for.</param>
        /// <param name="userIdentityId">The user id hased has been hashed</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="authenticatorToken"/> and <paramref name="userIdentityId"/> if it exists.
        /// </returns>
        public virtual async Task<UserFullDto> FindUserByIdentityIdAsync(string userIdentityId, string authenticationToken = null)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(userIdentityId))
            {
                throw new ArgumentNullException(nameof(userIdentityId));
            }

            var user = await UserStore.FindByIdentityIdAsync(userIdentityId);

            var tokenValidity = ValidateToken(user.Id, authenticationToken);
            if (tokenValidity.IsSucceed)
            {
                user.AuthenticationToken = tokenValidity.Result.ToString();
            }

            return user;
        }

        ///// <summary>
        ///// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        ///// called before saving the user via Create or Update.
        ///// </summary>
        ///// <param name="user">The user</param>
        ///// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        protected virtual async Task<ICommonResult> ValidateUserAsync(ApplicationUser user)
        {
            var errors = new List<CommonError>();
            foreach (var v in UserValidators)
            {
                var result = await v.ValidateAsync(this, user);
                if (!result.IsSucceed)
                {
                    errors.AddRange(result.Errors);
                }
            }

            if (errors.Count > 0)
            {
                return CommonResult.Failed(errors.ToArray());
            }
            return new CommonResult(true);
        }

        /// <summary>
        /// Gets the user identifier for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose identifier should be retrieved.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user"/>.</returns>
        public virtual async Task<string> GetUserIdAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            return await UserStore.GetUserIdAsync(user);
        }

        public virtual async Task<ICommonResult> CheckPasswordAsync(ApplicationUser user, string password)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var verifyResult = await VerifyPasswordAsync(user, password);
            if (verifyResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                await UpdatePasswordHash(user, password, validatePassword: false);
            }

            if (verifyResult == PasswordVerificationResult.Success || verifyResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                ModifyUserAuthenticate(user);
                return await UpdateAuthenticationAsync(user);
            }

            return CommonResult.Failed(new CommonError());
        }

        private void ModifyUserAuthenticate(ApplicationUser user)
        {
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{_tokenEncryptKey}"));
            var expiration = DateTime.UtcNow.AddMinutes(_tokenExpiryMinutes);

            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, $"{user.Email}{user.IdentityStamp}")
                },
                expires: expiration,
                signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature)
            );

            user.Expiration = expiration;
            user.AuthenticationToken = new JwtSecurityTokenHandler().WriteToken(token);
        }

        protected virtual async Task<ICommonResult> UpdateAuthenticationAsync(ApplicationUser user)
        {
            var result = await ValidateUserAsync(user);
            if (!result.IsSucceed)
            {
                return CommonResult.Failed(result.Errors.ToArray());
            }

            return await UserStore.UpdateAuthenticationAsync(user);
        }

        protected virtual async Task<PasswordVerificationResult> VerifyPasswordAsync(ApplicationUser user, string password)
        {
            var hash = await UserPasswordStore.GetPasswordHashAsync(user);
            if (hash == null)
            {
                return PasswordVerificationResult.Failed;
            }

            string passwordSalted = UserPasswordStore.AddSaltToPassword(user, password);
            return _passwordHasher.VerifyHashedPassword(user, hash, passwordSalted);
        }

        public virtual async Task<List<string>> GetRolesAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var userRoles = await UserStore.GetUserRolesAsync(user);
            return userRoles.ConvertAll(x => x.RoleName);
        }

        #endregion

        #region CRUD
        public virtual async Task<ICommonResult> CreateAsync(ApplicationUser user)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Password == null)
            {
                throw new ArgumentNullException(nameof(user.Password));
            }

            var data = await ValidateUserAsync(user);
            if (!data.IsSucceed)
            {
                return data;
            }

            user.PasswordSalt = UserStampStore.NewSecuritySalt();
            var isUpdated = await UpdatePasswordHash(user, user.Password);
            if (!isUpdated)
            {
                return data;
            }

            user.ActiveUserStamp = UserStampStore.NewSecurityStamp();
            var response = await UserStore.CreateAsync(user);
            if (response.IsSucceed)
            {
                var userResponse = response.Result as ApplicationUser;
                userResponse.ActiveUserStamp = user.ActiveUserStamp;
                userResponse.PasswordSalt = user.PasswordSalt;

                var newUserAttributes = UserStampStore.NewUserRegisterAttributes(userResponse);
                await UserStampStore.SetAttributesAsync(newUserAttributes);
            }

            return response;
        }

        public virtual async Task<UserTokenResult> ChangePasswordAsync(long userId, string currentPassword, string newPassword)
        {
            ThrowIfDisposed();
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                var user = await UserStore.FindByIdAsync(userId);
                user.PasswordSalt = await UserStampStore.GetPasswordSaltAsync(user.Id);
                if (user == null || !(await VerifyPasswordAsync(user, currentPassword) != PasswordVerificationResult.Failed))
                {
                    throw new CocoApplicationException(Describer.PasswordMismatch());
                }

                var isUpdated = await UpdatePasswordHash(user, newPassword);
                if (!isUpdated)
                {
                    return new UserTokenResult(false);
                }

                return await UserPasswordStore.ChangePasswordAsync(user.Id, currentPassword, user.PasswordHash);
            }
            catch (Exception e)
            {
                throw new CocoApplicationException(e);
            }
        }

        /// <summary>
        /// Changes a user's password after confirming the specified <paramref name="currentPassword"/> is correct,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="userId">The user id whose password should be set.</param>
        /// <param name="currentPassword">The current password to validate before changing.</param>
        /// <param name="newPassword">The new password to set for the specified</param>
        /// <param name="key">The new password to set for the specified</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<UserTokenResult> ResetPasswordAsync(ResetPasswordModel model, long userId)
        {
            ThrowIfDisposed();
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                var user = await UserStore.FindByIdAsync(userId);
                if (user == null || !(await IsEmailConfirmedAsync(user)))
                {
                    throw new UnauthorizedAccessException("ResetPasswordFailed");
                }

                var isResetKeyValid = await IsResetPasswordKeyMatched(user.Id, model.Key);
                if (!isResetKeyValid)
                {
                    throw new UnauthorizedAccessException("TheResetKeyIsMismatched");
                }

                user.PasswordSalt = await UserStampStore.GetPasswordSaltAsync(user.Id);
                if(user == null || !(await VerifyPasswordAsync(user, model.CurrentPassword) != PasswordVerificationResult.Failed))
                {
                    throw new CocoApplicationException(Describer.PasswordMismatch());
                }

                var isUpdated = await UpdatePasswordHash(user, model.Password);
                if (!isUpdated)
                {
                    return new UserTokenResult(false);
                }

                var result = await UserPasswordStore.ChangePasswordAsync(user.Id, model.CurrentPassword, user.PasswordHash);
                if (result.IsSucceed)
                {
                    await UserStampStore.DeleteResetPasswordByEmailAttribute(user);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new CocoApplicationException(e);
            }
        }

        private async Task<bool> IsResetPasswordKeyMatched(long userId, string key)
        {
            var result = await UserStampStore.GetResetPasswordKeyAsync(userId);
            return result.Equals(key);
        }

        /// <summary>
        /// Creates the specified <paramref name="model"/> in the backing store with no password,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="model">The user to update by item.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<UpdatePerItemModel> UpdateInfoItemAsync(UpdatePerItemModel model, string userIdentityId, string token)
        {
            ThrowIfDisposed();

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.PropertyName == null)
            {
                throw new ArgumentNullException(nameof(model.PropertyName));
            }

            if (model.Key == null || string.IsNullOrEmpty(model.Key.ToString()))
            {
                throw new ArgumentNullException(nameof(model.Key));
            }

            var user = await FindUserByIdentityIdAsync(userIdentityId, token);
            if (user == null || !user.AuthenticationToken.Equals(token))
            {
                throw new UnauthorizedAccessException(nameof(user));
            }

            return await UserStore.UpdateInfoItemAsync(model);
        }

        public virtual async Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(ApplicationUser user, string userIdentityId, string token)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var result = await FindUserByIdentityIdAsync(userIdentityId, token);
            if (result == null || !result.AuthenticationToken.Equals(token))
            {
                throw new UnauthorizedAccessException(nameof(user));
            }

            return await UserStore.UpdateIdentifierAsync(user);
        }

        public virtual async Task<UserPhotoUpdateDto> UpdateAvatarAsync(UserPhotoUpdateDto model, ApplicationUser user)
        {
            ThrowIfDisposed();

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var currentUser = await FindUserByIdentityIdAsync(user.UserIdentityId, user.AuthenticationToken);
            if(currentUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            return await UserPhotoStore.UpdateAvatarAsync(model, currentUser.Id);
        }

        public virtual async Task<UserPhotoUpdateDto> UpdateCoverAsync(UserPhotoUpdateDto model, ApplicationUser user)
        {
            ThrowIfDisposed();

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var currentUser = await FindUserByIdentityIdAsync(user.UserIdentityId, user.AuthenticationToken);
            if (currentUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            return await UserPhotoStore.UpdateCoverAsync(model, currentUser.Id);
        }

        public virtual async Task<UserPhotoUpdateDto> DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType)
        {
            ThrowIfDisposed();

            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await UserPhotoStore.DeleteUserPhotoAsync(userId, userPhotoType);
        }

        public virtual async Task<ICommonResult> ForgotPasswordAsync(string email)
        {
            ThrowIfDisposed();

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var user = await FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var resetAttribute = await UserStampStore.SetResetPasswordStampAsync(user, UserStampStore.NewSecurityStamp());
            user.ActiveUserStamp = resetAttribute.Value;

            return await UserEmailStore.SendForgotPasswordAsync(user);
        }


        public virtual async Task<bool> ClearUserLoginAsync(string userIdentityId, string authenticationToken)
        {
            ThrowIfDisposed();

            var user = await FindUserByIdentityIdAsync(userIdentityId, authenticationToken);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var applicationUser = _mapper.Map<ApplicationUser>(user);
            return await ClearUserLoginAsync(applicationUser, authenticationToken);
        }

        private async Task<bool> ClearUserLoginAsync(ApplicationUser user, string authenticationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(authenticationToken))
            {
                throw new ArgumentNullException(nameof(authenticationToken));
            }

            var applicationUser = _mapper.Map<ApplicationUser>(user);
            return await UserStampStore.DeleteUserAuthenticationAttributes(applicationUser, authenticationToken);
        }

        public async Task<ICommonResult> ActiveAsync(string email, string activeKey)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(activeKey))
            {
                throw new ArgumentNullException(nameof(activeKey));
            }

            var user = await FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.IsActived)
            {
                return CommonResult.Failed(Describer.UserAlreadyActived());
            }

            var activationAttribute = await UserStampStore.GetActivationKeyAsync(user.Id);
            if (string.IsNullOrEmpty(activationAttribute))
            {
                throw new ArgumentNullException(nameof(activationAttribute));
            }

            if (!activationAttribute.Equals(activeKey))
            {
                throw new UnauthorizedAccessException(nameof(activeKey));
            }

            var result = await UserStore.ActiveAsync(user);

            if (result.IsSucceed)
            {
                await UserStampStore.DeleteUserActivationAttribute(user);
            }

            return result;
        }

        private ICommonResult ValidateToken(long userId, string token)
        {
            var tokenResult = UserStampStore.GetAuthenticationAttribute(userId, token);
            bool hasToken = tokenResult != null && !string.IsNullOrEmpty(tokenResult.AuthenticationToken);
            bool isValid = hasToken && tokenResult.AuthenticationToken.Equals(token);

            if (!isValid)
            {
                return CommonResult.Failed(new CommonError() { 
                    Code = ErrorMessageConst.UN_EXPECTED_EXCEPTION,
                    Message = ErrorMessageConst.UN_EXPECTED_EXCEPTION
                });
            }

            return CommonResult.Success(tokenResult.AuthenticationToken, isValid);
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Releases all resources used by the user manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                //Store.Dispose();
                _isDisposed = true;
            }
        }

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        #endregion
    }
}
