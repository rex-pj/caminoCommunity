using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Api.Framework.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coco.Api.Framework.Commons.Enums;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Coco.Entities.Model.General;
using Coco.Entities.Model.User;
using Coco.Entities.Enums;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using Coco.Api.Framework.Commons.Encode;
using Coco.Api.Framework.Services.Contracts;
using Coco.Api.Framework.SessionManager.Core;

namespace Coco.Api.Framework.SessionManager
{
    public class UserManager : IUserManager<ApplicationUser>
    {
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        
        #region Properties
        public const string ResetPasswordTokenPurpose = "ResetPassword";
        public const string ConfirmEmailTokenPurpose = "EmailConfirmation";
        internal readonly string _tokenEncryptKey;
        internal readonly int _tokenExpiryMinutes;
        protected internal IUserStore<ApplicationUser> UserStore;
        protected internal IUserPhotoStore<ApplicationUser> UserPhotoStore;
        protected internal IUserPasswordStore<ApplicationUser> UserPasswordStore;
        protected internal IUserEmailStore<ApplicationUser> UserEmailStore;
        protected internal IUserSecurityStampStore<ApplicationUser> SecurityStampStore;
        
        public IList<IUserValidator<ApplicationUser>> UserValidators { get; } = new List<IUserValidator<ApplicationUser>>();
        public IList<IPasswordValidator<ApplicationUser>> PasswordValidators { get; } = new List<IPasswordValidator<ApplicationUser>>();
        public IdentityOptions Options { get; set; }
        public IdentityErrorDescriber Describer { get; private set; }
        protected virtual CancellationToken CancellationToken => CancellationToken.None;
        #endregion

        #region Fields
        private bool _isDisposed;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly Dictionary<string, IUserTokenProvider<ApplicationUser>> _tokenProviders =
            new Dictionary<string, IUserTokenProvider<ApplicationUser>>();
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
            IServiceProvider services,
            IUserSecurityStampStore<ApplicationUser> userSecurityStampStore,
            IdentityErrorDescriber errors = null)
        {
            this.Options = optionsAccessor?.Value ?? new IdentityOptions();
            this.UserStore = userStore;
            this.UserPhotoStore = userPhotoStore;
            this.UserEmailStore = userEmailStore;
            this.UserPasswordStore = userPasswordStore;
            this.SecurityStampStore = userSecurityStampStore;

            _passwordHasher = passwordHasher;
            _lookupNormalizer = lookupNormalizer;
            _tokenEncryptKey = configuration["Jwt:SecretKey"];
            _tokenExpiryMinutes = Convert.ToInt32(configuration["Jwt:ExpiryMinutes"]);
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

            if (services != null)
            {
                foreach (var providerName in Options.Tokens.ProviderMap.Keys)
                {
                    var description = Options.Tokens.ProviderMap[providerName];

                    var provider = (description.ProviderInstance ?? services.GetRequiredService(description.ProviderType))
                        as IUserTokenProvider<ApplicationUser>;
                    if (provider != null)
                    {
                        RegisterTokenProvider(providerName, provider);
                    }
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates a user's password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="validatePassword">Whether to validate the password.</param>
        /// <returns>Whether the password has was successfully updated.</returns>
        protected virtual async Task<ApiResult> UpdatePasswordHash(ApplicationUser user, string newPassword, bool validatePassword = true)
        {
            if (validatePassword)
            {
                var validate = await ValidatePasswordAsync(user, newPassword);
                if (!validate.IsSuccess)
                {
                    return validate;
                }
            }

            // Custom password hashing
            string passwordHashed = null;
            if (!string.IsNullOrEmpty(newPassword))
            {
                string passwordSalted = UserPasswordStore.AddSaltToPassword(user, newPassword);
                passwordHashed = _passwordHasher.HashPassword(user, passwordSalted);
            }

            await UserPasswordStore.SetPasswordHashAsync(user, passwordHashed, CancellationToken);
            return new ApiResult(true);
        }

        /// <summary>
        /// Should return <see cref="ApiResult.Success"/> if validation is successful. This is
        /// called before updating the password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>A <see cref="ApiResult"/> representing whether validation was successful.</returns>
        protected async Task<ApiResult> ValidatePasswordAsync(ApplicationUser user, string password)
        {
            var errors = new List<ApiError>();
            var isValid = true;
            foreach (var v in PasswordValidators)
            {
                var result = await v.ValidateAsync(this, user, password);
                if (!result.IsSuccess)
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
                return ApiResult.Failed(errors.ToArray());
            }
            return new ApiResult(true);
        }

        /// <summary>
        /// Gets the email address for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose email should be returned.</param>
        /// <returns>The task object containing the results of the asynchronous operation, the email address for the specified <paramref name="user"/>.</returns>
        public virtual async Task<string> GetEmailAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return await UserEmailStore.GetEmailAsync(user, CancellationToken);
        }

        /// <summary>
        /// Normalize user or role name for consistent comparisons.
        /// </summary>
        /// <param name="name">The name to normalize.</param>
        /// <returns>A normalized value representing the specified <paramref name="name"/>.</returns>
        public virtual string NormalizeName(string name)
        {
            return (_lookupNormalizer == null) ? name : _lookupNormalizer.NormalizeName(name);
        }

        /// <summary>
        /// Normalize email for consistent comparisons.
        /// </summary>
        /// <param name="email">The email to normalize.</param>
        /// <returns>A normalized value representing the specified <paramref name="email"/>.</returns>
        public virtual string NormalizeEmail(string email)
        {
            return (_lookupNormalizer == null) ? email : _lookupNormalizer.NormalizeEmail(email);
        }

        /// <summary>
        /// Gets the user name for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose name should be retrieved.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the name for the specified <paramref name="user"/>.</returns>
        public virtual async Task<string> GetUserNameAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return await UserStore.GetUserNameAsync(user, CancellationToken);
        }

        /// <summary>
        /// Gets the user, if any, associated with the normalized value of the specified email address.
        /// Note: Its recommended that identityOptions.User.RequireUniqueEmail be set to true when using this method, otherwise
        /// the store may throw if there are users with duplicate emails.
        /// </summary>
        /// <param name="email">The email address to return the user for.</param>
        /// <returns>
        /// The task object containing the results of the asynchronous lookup operation, the user, if any, associated with a normalized value of the specified email address.
        /// </returns>
        public virtual async Task<ApplicationUser> FindByEmailAsync(string email, bool includeInActived = false)
        {
            ThrowIfDisposed();
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            email = NormalizeEmail(email);
            var user = await UserStore.FindByEmailAsync(email, includeInActived, CancellationToken);
            return user;

        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified user name.
        /// </summary>
        /// <param name="userName">The user name to search for.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userName"/> if it exists.
        /// </returns>
        public virtual async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            userName = NormalizeName(userName);

            ApplicationUser user = await UserStore.FindByNameAsync(userName, CancellationToken);
            return user;
        }

        /// <summary>
        /// Gets a flag indicating whether the email address for the specified <paramref name="user"/> has been verified, true if the email address is verified otherwise
        /// false.
        /// </summary>
        /// <param name="user">The user whose email confirmation status should be returned.</param>
        /// <returns>
        /// The task object containing the results of the asynchronous operation, a flag indicating whether the email address for the specified <paramref name="user"/>
        /// has been confirmed or not.
        /// </returns>
        public virtual async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return await UserEmailStore.GetEmailConfirmedAsync(user, CancellationToken);
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

            var user = UserStore.FindByIdentityId(userIdentityId, CancellationToken);

            if (!user.AuthenticationToken.Equals(authenticationToken) && !user.IsActived)
            {
                throw new UnauthorizedAccessException();
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
        public virtual async Task<UserFullModel> GetFullByHashIdAsync(string userIdentityId)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(userIdentityId))
            {
                throw new ArgumentNullException(nameof(userIdentityId));
            }

            var user = await UserStore.GetFullByFindByHashedIdAsync(userIdentityId, CancellationToken);
            return user;
        }

        ///// <summary>
        ///// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        ///// called before saving the user via Create or Update.
        ///// </summary>
        ///// <param name="user">The user</param>
        ///// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        protected virtual async Task<ApiResult> ValidateUserAsync(ApplicationUser user)
        {
            var errors = new List<ApiError>();
            foreach (var v in UserValidators)
            {
                var result = await v.ValidateAsync(this, user);
                if (!result.IsSuccess)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                return ApiResult.Failed(errors.ToArray());
            }
            return new ApiResult(true);
        }

        /// <summary>
        /// Gets the user identifier for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose identifier should be retrieved.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user"/>.</returns>
        public virtual async Task<string> GetUserIdAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            return await UserStore.GetUserIdAsync(user, CancellationToken);
        }

        /// <summary>
        /// Returns a flag indicating whether the given <paramref name="password"/> is valid for the
        /// specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password to validate</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing true if
        /// the specified <paramref name="password" /> matches the one store for the <paramref name="user"/>,
        /// otherwise false.</returns>
        public virtual async Task<ApiResult> CheckPasswordAsync(ApplicationUser user, string password)
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
                ModifyUserAuthenticate(user);
                var result = await UpdateAuthenticationAsync(user);
                return result;
            }
            else if (verifyResult == PasswordVerificationResult.Success)
            {
                ModifyUserAuthenticate(user);
                var result = await UpdateAuthenticationAsync(user);
                return result;
            }

            var success = verifyResult != PasswordVerificationResult.Failed;
            return ApiResult.Success(success);
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

        /// <summary>
        /// Returns a <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="store">The store containing a user's password.</param>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="PasswordVerificationResult"/>
        /// of the operation.
        /// </returns>
        protected virtual async Task<PasswordVerificationResult> VerifyPasswordAsync(ApplicationUser user, string password)
        {
            var hash = await UserPasswordStore.GetPasswordHashAsync(user, CancellationToken);
            if (hash == null)
            {
                return PasswordVerificationResult.Failed;
            }

            string passwordSalted = UserPasswordStore.AddSaltToPassword(user, password);

            return _passwordHasher.VerifyHashedPassword(user, hash, passwordSalted);
        }

        #endregion

        #region CRUD
        /// <summary>
        /// Creates the specified <paramref name="user"/> in the backing store with no password,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<ApiResult> CreateAsync(ApplicationUser user)
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
            if (!data.IsSuccess)
            {
                return data;
            }

            var updatePasswordResult = await UpdatePasswordHash(user, user.Password);
            if (!updatePasswordResult.IsSuccess)
            {
                return data;
            }

            await UpdateSecurityStampInternal(user);
            await UpdateIdentityStampInternal(user);
            await UpdatePasswordSaltInternal(user);

            var result = await UserStore.CreateAsync(user, CancellationToken);

            return result;
        }

        private async Task UpdateSecurityStampInternal(ApplicationUser user)
        {
            await SecurityStampStore.SetSecurityStampAsync(user, NewSecurityStamp(), CancellationToken);
        }

        private async Task UpdateIdentityStampInternal(ApplicationUser user)
        {
            await SecurityStampStore.SetIdentityStampAsync(user, NewIdentityStamp(), CancellationToken);
        }

        private async Task UpdatePasswordSaltInternal(ApplicationUser user)
        {
            await SecurityStampStore.SetPasswordSaltAsync(user, NewIdentityStamp(), CancellationToken);
        }

        private static string NewSecurityStamp()
        {
            byte[] bytes = new byte[20];
            _rng.GetBytes(bytes);
            return Base32.ToBase32(bytes);
        }

        private static string NewIdentityStamp()
        {
            return SaltGenerator.GetSalt();
        }

        /// <summary>
        /// Called to update the user after validating and updating the normalized email/user name.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>Whether the operation was successful.</returns>
        protected virtual async Task<ApiResult> UpdateAuthenticationAsync(ApplicationUser user)
        {
            var result = await ValidateUserAsync(user);
            if (!result.IsSuccess)
            {
                return ApiResult.Failed(result.Errors.ToArray());
            }

            return await UserStore.UpdateAuthenticationAsync(user, CancellationToken);
        }

        /// <summary>
        /// Changes a user's password after confirming the specified <paramref name="currentPassword"/> is correct,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose password should be set.</param>
        /// <param name="currentPassword">The current password to validate before changing.</param>
        /// <param name="newPassword">The new password to set for the specified <paramref name="user"/>.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<ApiResult> ChangePasswordAsync(long userId, string currentPassword, string newPassword)
        {
            ThrowIfDisposed();
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                var user = await UserStore.FindByIdAsync(userId, CancellationToken);
                if (user != null && await VerifyPasswordAsync(user, currentPassword) != PasswordVerificationResult.Failed)
                {
                    var data = await UpdatePasswordHash(user, newPassword);
                    if (!data.IsSuccess)
                    {
                        return data;
                    }

                    var result = await UserPasswordStore.ChangePasswordAsync(user.Id, currentPassword, user.PasswordHash);

                    return ApiResult<UserTokenResult>.Success(result);
                }

                return ApiResult.Failed(Describer.PasswordMismatch());
            }
            catch (Exception e)
            {
                return ApiResult<UserTokenResult>.Failed(new ApiError()
                {
                    Description = e.Message
                }, new UserTokenResult()
                {
                    IsSuccess = false
                });
            }
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
        public virtual async Task<ApiResult> UpdateInfoItemAsync(UpdatePerItemModel model, string userIdentityId, string token)
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

            var user = await GetFullByHashIdAsync(userIdentityId);
            if (user == null || !user.AuthenticationToken.Equals(token))
            {
                throw new UnauthorizedAccessException(nameof(user));
            }

            return await UserStore.UpdateInfoItemAsync(model, CancellationToken);
        }

        public virtual async Task<ApiResult> UpdateUserProfileAsync(ApplicationUser user, string userIdentityId, string token)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var result = await GetFullByHashIdAsync(userIdentityId);
            if (result == null || !result.AuthenticationToken.Equals(token))
            {
                throw new UnauthorizedAccessException(nameof(user));
            }

            return await UserStore.UpdateUserProfileAsync(user, CancellationToken);
        }

        public virtual async Task<ApiResult> UpdateAvatarAsync(UpdateUserPhotoModel model, long userId)
        {
            ThrowIfDisposed();

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return await UserPhotoStore.UpdateAvatarAsync(model, userId, CancellationToken);
        }

        public virtual async Task<ApiResult> UpdateCoverAsync(UpdateUserPhotoModel model, long userId)
        {
            ThrowIfDisposed();

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return await UserPhotoStore.UpdateCoverAsync(model, userId, CancellationToken);
        }

        public virtual async Task<ApiResult> DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType)
        {
            ThrowIfDisposed();

            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await UserPhotoStore.DeleteUserPhotoAsync(userId, userPhotoType, CancellationToken);
        }

        public virtual async Task<ApiResult> ForgotPasswordAsync(string email)
        {
            ThrowIfDisposed();

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var user = await FindByEmailAsync(email, true);
            if(user == null)
            {
                throw new NullReferenceException(nameof(user));
            }

            return await UserEmailStore.SendForgotPasswordAsync(email, CancellationToken);
        }

        public virtual async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            return await GenerateUserTokenAsync(user, Options.Tokens.EmailConfirmationTokenProvider, ConfirmEmailTokenPurpose);
        }

        public virtual async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            return await GenerateUserTokenAsync(user, Options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose);
        }

        /// <summary>
        /// Generates a token for the given <paramref name="user"/> and <paramref name="purpose"/>.
        /// </summary>
        /// <param name="purpose">The purpose the token will be for.</param>
        /// <param name="user">The user the token will be for.</param>
        /// <param name="tokenProvider">The provider which will generate the token.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents result of the asynchronous operation, a token for
        /// the given user and purpose.
        /// </returns>
        public virtual async Task<string> GenerateUserTokenAsync(ApplicationUser user, string tokenProvider, string purpose)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (tokenProvider == null)
            {
                throw new ArgumentNullException(nameof(tokenProvider));
            }
            if (!_tokenProviders.ContainsKey(tokenProvider))
            {
                throw new NotSupportedException($"{nameof(ApplicationUser)}: {tokenProvider}");
            }

            var securityToken = await CreateSecurityTokenAsync(user);
            return await _tokenProviders[tokenProvider].GenerateAsync(purpose, this, user);
        }

        /// <summary>
        /// Get the security stamp for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose security stamp should be set.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the security stamp for the specified <paramref name="user"/>.</returns>
        public virtual async Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var stamp = await SecurityStampStore.GetSecurityStampAsync(user, CancellationToken);
            if (stamp == null)
            {
                throw new InvalidOperationException(nameof(stamp));
            }
            return stamp;
        }

        /// <summary>
        /// Creates bytes to use as a security token from the user's security stamp.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The security token bytes.</returns>
        public virtual async Task<byte[]> CreateSecurityTokenAsync(ApplicationUser user)
        {
            return Encoding.Unicode.GetBytes(await GetSecurityStampAsync(user));
        }

        /// <summary>
        /// Registers a token provider.
        /// </summary>
        /// <param name="providerName">The name of the provider to register.</param>
        /// <param name="provider">The provider to register.</param>
        public virtual void RegisterTokenProvider(string providerName, IUserTokenProvider<ApplicationUser> provider)
        {
            ThrowIfDisposed();
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            _tokenProviders[providerName] = provider;
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
