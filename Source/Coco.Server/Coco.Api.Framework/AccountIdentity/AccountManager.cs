using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.AccountIdentity.Entities;
using Microsoft.Extensions.DependencyInjection;
using Coco.Api.Framework.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coco.Api.Framework.AccountIdentity.Commons.Enums;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Coco.Api.Framework.AccountIdentity
{
    public class AccountManager : IAccountManager<ApplicationUser>
    {
        #region Properties
        /// <summary>
        /// Gets or sets the persistence store the manager operates over.
        /// </summary>
        /// <value>The persistence store the manager operates over.</value>
        protected internal IUserStore<ApplicationUser> UserStore;
        protected internal IUserPasswordStore<ApplicationUser> UserPasswordStore;
        protected internal IUserEmailStore<ApplicationUser> UserEmailStore;
        internal readonly string _tokenEncryptKey;
        internal readonly int _tokenExpiryMinutes;
        public IList<IUserValidator<ApplicationUser>> UserValidators { get; } = new List<IUserValidator<ApplicationUser>>();
        public IList<IPasswordValidator<ApplicationUser>> PasswordValidators { get; } = new List<IPasswordValidator<ApplicationUser>>();
        public IdentityOptions Options { get; set; }
        /// <summary>
        /// The cancellation token used to cancel operations.
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;
        #endregion

        #region Fields
        private bool _isDisposed;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IServiceProvider _services;
        #endregion

        #region Ctor
        public AccountManager(IUserStore<ApplicationUser> userStore,
            IUserEmailStore<ApplicationUser> userEmailStore,
            IUserPasswordStore<ApplicationUser> userPasswordStore,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IOptions<IdentityOptions> optionsAccessor,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IServiceProvider services,
            IConfiguration configuration,
            ILookupNormalizer lookupNormalizer)
        {
            this.Options = optionsAccessor?.Value ?? new IdentityOptions();
            this.UserStore = userStore;
            this.UserEmailStore = userEmailStore;
            this.UserPasswordStore = userPasswordStore;

            _services = services;
            _passwordHasher = passwordHasher;
            _lookupNormalizer = lookupNormalizer;
            _tokenEncryptKey = configuration["Jwt:SecretKey"];
            _tokenExpiryMinutes = Convert.ToInt32(configuration["Jwt:ExpiryMinutes"]);

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
        /// <summary>
        /// Creates the specified <paramref name="user"/> in the backing store with no password,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<IdentityResult> CreateAsync(ApplicationUser user)
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

            var result = await ValidateUserAsync(user);
            if (!result.IsSuccess)
            {
                return result;
            }

            var updatePasswordResult = await UpdatePasswordHash(user, user.Password);
            if (!updatePasswordResult.IsSuccess)
            {
                return result;
            }

            return await UserStore.CreateAsync(user, CancellationToken);
        }

        /// <summary>
        /// Updates a user's password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="validatePassword">Whether to validate the password.</param>
        /// <returns>Whether the password has was successfully updated.</returns>
        protected virtual async Task<IdentityResult> UpdatePasswordHash(ApplicationUser user, string newPassword, bool validatePassword = true)
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
            return new IdentityResult(true);
        }

        /// <summary>
        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        /// called before updating the password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        protected async Task<IdentityResult> ValidatePasswordAsync(ApplicationUser user, string password)
        {
            var errors = new List<IdentityError>();
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
                return IdentityResult.Failed(errors.ToArray());
            }
            return new IdentityResult(true);
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
        public virtual async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            email = NormalizeEmail(email);
            var user = await UserStore.FindByEmailAsync(email, CancellationToken);

            // Need to potentially check all keys
            if (user == null && Options.Stores.ShouldProtectPersonalData)
            {
                var keyRing = _services.GetService<ILookupProtectorKeyRing>();
                var protector = _services.GetService<ILookupProtector>();
                if (keyRing != null && protector != null)
                {
                    foreach (var key in keyRing.GetAllKeyIds())
                    {
                        var oldKey = protector.Protect(key, email);
                        user = await UserStore.FindByEmailAsync(oldKey, CancellationToken);
                        if (user != null)
                        {
                            return user;
                        }
                    }
                }
            }
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

            //// Need to potentially check all keys
            if (user == null && Options.Stores.ShouldProtectPersonalData)
            {
                var keyRing = _services.GetService<ILookupProtectorKeyRing>();
                var protector = _services.GetService<ILookupProtector>();
                if (keyRing != null && protector != null)
                {
                    foreach (var key in keyRing.GetAllKeyIds())
                    {
                        var oldKey = protector.Protect(key, userName);
                        user = await UserStore.FindByNameAsync(oldKey, CancellationToken);
                        if (user != null)
                        {
                            return user;
                        }
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified authenticator token and user id in hashed.
        /// </summary>
        /// <param name="authenticatorToken">The authenticator token to search for.</param>
        /// <param name="userIdHased">The user id hased has been hashed</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="authenticatorToken"/> and <paramref name="userIdHased"/> if it exists.
        /// </returns>
        public virtual async Task<ApplicationUser> FindByTokenAsync(string userIdHased, string authenticatorToken)
        {
            ThrowIfDisposed();
            if (authenticatorToken == null)
            {
                throw new ArgumentNullException(nameof(authenticatorToken));
            }

            var user = await UserStore.FindByTokenAsync(userIdHased, authenticatorToken, CancellationToken);
            return user;
        }

        ///// <summary>
        ///// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        ///// called before saving the user via Create or Update.
        ///// </summary>
        ///// <param name="user">The user</param>
        ///// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        protected virtual async Task<IdentityResult> ValidateUserAsync(ApplicationUser user)
        {
            var errors = new List<IdentityError>();
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
                return IdentityResult.Failed(errors.ToArray());
            }
            return new IdentityResult(true);
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
        public virtual async Task<LoginResult> CheckPasswordAsync(ApplicationUser user, string password)
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
                UpdateUserAuthenticate(user);
                var result = await UpdateUserAsync(user);
                return result;
            }
            else if (verifyResult == PasswordVerificationResult.Success)
            {
                UpdateUserAuthenticate(user);
                var result = await UpdateUserAsync(user);
                return result;
            }

            var success = verifyResult != PasswordVerificationResult.Failed;
            return new LoginResult(success);
        }

        /// <summary>
        /// Called to update the user after validating and updating the normalized email/user name.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>Whether the operation was successful.</returns>
        protected virtual async Task<LoginResult> UpdateUserAsync(ApplicationUser user)
        {
            var result = await ValidateUserAsync(user);
            if (!result.IsSuccess)
            {
                return LoginResult.Failed(result.Errors.ToArray());
            }

            return await UserStore.UpdateAsync(user, CancellationToken);
        }

        private void UpdateUserAuthenticate(ApplicationUser user)
        {
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{_tokenEncryptKey}"));
            var expiration = DateTime.UtcNow.AddMinutes(_tokenExpiryMinutes);

            var token = new JwtSecurityToken(
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, $"{user.Email}{user.SecurityStamp}")
                },
                expires: expiration,
                signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature)
            );

            user.Expiration = expiration;
            user.AuthenticatorToken = new JwtSecurityTokenHandler().WriteToken(token);
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

        #region Privates
        private string ProtectPersonalData(string data)
        {
            if (Options.Stores.ShouldProtectPersonalData)
            {
                var keyRing = _services.GetService<ILookupProtectorKeyRing>();
                var protector = _services.GetService<ILookupProtector>();
                return protector.Protect(keyRing.CurrentKeyId, data);
            }
            return data;
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
