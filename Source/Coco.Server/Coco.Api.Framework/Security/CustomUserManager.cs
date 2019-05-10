using Coco.Api.Framework.Commons.Encode;
using Coco.Api.Framework.Commons.ErrorMessage;
using Coco.Api.Framework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Coco.Api.Framework.Security
{
    public class CustomUserManager : UserManager<ApplicationUser>
    {
        #region Fields
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        // <summary>
        /// The <see cref="IPasswordHasher{TUser}"/> used to hash passwords.
        /// </summary>
        private IPasswordHasher<ApplicationUser> _passwordHasher { get; set; }
        internal readonly string _encryptKey;
        private readonly IServiceProvider _services;
        #endregion

        #region Ctor
        public CustomUserManager(IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger, IConfiguration configuration)
            : base(store, optionsAccessor, passwordHasher, userValidators,
              passwordValidators, keyNormalizer, errors, services, logger)
        {
            _encryptKey = configuration.GetValue<string>("EncryptKey");
            _services = services;
            KeyNormalizer = keyNormalizer;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Normalize user or role name for consistent comparisons.
        /// </summary>
        /// <param name="name">The name to normalize.</param>
        /// <returns>A normalized value representing the specified <paramref name="name"/>.</returns>
        public virtual string NormalizeName(string name)
        {
            return (KeyNormalizer == null) ? name : KeyNormalizer.Normalize(name);
        }

        /// <summary>
        /// Creates the specified <paramref name="user"/> in the backing store with given password,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="password">The password for the user to hash and store.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            ThrowIfDisposed();
            var passwordStore = GetPasswordStore();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var result = await UpdatePasswordHash(passwordStore, user, password);
            if (!result.Succeeded)
            {
                return result;
            }

            return await CreateAsync(user);
        }

        /// <summary>
        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        /// called before saving the user via Create or Update.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        /// Todo: Re-Comments
        protected new async Task<IdentityResult> ValidateUserAsync(ApplicationUser user)
        {
            if (SupportsUserSecurityStamp)
            {
                var stamp = await GetSecurityStampAsync(user);
                if (stamp == null)
                {
                    throw new InvalidOperationException(ExceptionMessageConst.NullSecurityStamp);
                }
            }
            var errors = new List<IdentityError>();
            foreach (var v in UserValidators)
            {
                var result = await v.ValidateAsync(this, user);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                Logger.LogWarning(13, "User {userId} validation failed: {errors}.", await GetUserIdAsync(user),
                    string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
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
        public override async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            ThrowIfDisposed();
            var passwordStore = GetPasswordStore();
            if (user == null)
            {
                return false;
            }

            var result = await VerifyPasswordAsync(passwordStore, user, password);
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                await UpdatePasswordHash(passwordStore, user, password, validatePassword: false);
                await UpdateUserAsync(user);
            }

            var success = result != PasswordVerificationResult.Failed;
            if (!success)
            {
                Logger.LogWarning(0, "Invalid password for user {userId}.", await GetUserIdAsync(user));
            }
            return success;
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
        protected override async Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<ApplicationUser> store, ApplicationUser user, string password)
        {
            var hash = await store.GetPasswordHashAsync(user, CancellationToken);
            if (hash == null)
            {
                return PasswordVerificationResult.Failed;
            }
            return PasswordHasher.VerifyHashedPassword(user, hash, password);
        }

        /// <summary>
        /// Get the security stamp for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose security stamp should be set.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the security stamp for the specified <paramref name="user"/>.</returns>
        public override async Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            var securityStore = GetSecurityStore();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var stamp = await securityStore.GetSecurityStampAsync(user, CancellationToken);
            if (stamp == null)
            {
                Logger.LogWarning(15, "GetSecurityStampAsync for user {userId} failed because stamp was null.", await GetUserIdAsync(user));
                throw new InvalidOperationException("NullSecurityStamp");
            }
            return stamp;
        }

        /// <summary>
        /// Returns the Name claim value if present otherwise returns null.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
        /// <returns>The Name claim value, or null if the claim is not present.</returns>
        /// <remarks>The Name claim is identified by <see cref="ClaimsIdentity.DefaultNameClaimType"/>.</remarks>
        public virtual string GetUserName(CustomClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            return principal.FindFirstValue(Options.ClaimsIdentity.UserNameClaimType);
        }

        /// <summary>
        /// Returns the User ID claim value if present otherwise returns null.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
        /// <returns>The User ID claim value, or null if the claim is not present.</returns>
        /// <remarks>The User ID claim is identified by <see cref="ClaimTypes.NameIdentifier"/>.</remarks>
        public virtual string GetUserId(CustomClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            return principal.FindFirstValue(Options.ClaimsIdentity.UserIdClaimType);
        }

        /// <summary>
        /// Returns the user corresponding to the IdentityOptions.ClaimsIdentity.UserIdClaimType claim in
        /// the principal or null.
        /// </summary>
        /// <param name="principal">The principal which contains the user id claim.</param>
        /// <returns>The user corresponding to the IdentityOptions.ClaimsIdentity.UserIdClaimType claim in
        /// the principal or null</returns>
        /// Todo: Re-Comments
        public virtual Task<ApplicationUser> GetUserAsync(CustomClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var id = GetUserId(principal);
            return id == null ? Task.FromResult<ApplicationUser>(null) : FindByIdAsync(id);
        }

        /// <summary>
        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        /// called before updating the password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        protected new async Task<IdentityResult> ValidatePasswordAsync(ApplicationUser user, string password)
        {
            var errors = new List<IdentityError>();
            foreach (var v in PasswordValidators)
            {
                var result = await v.ValidateAsync(this, user, password);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                Logger.LogWarning(14, "User {userId} password validation failed: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        #endregion

        #region Privates
        private async Task<IdentityResult> UpdatePasswordHash(IUserPasswordStore<ApplicationUser> passwordStore,
            ApplicationUser user, string newPassword, bool validatePassword = true)
        {
            if (validatePassword)
            {
                var validate = await ValidatePasswordAsync(user, newPassword);
                if (!validate.Succeeded)
                {
                    return validate;
                }
            }

            // Custom password hashing
            string passwordHashed = null;
            if (!string.IsNullOrEmpty(newPassword))
            {
                string passwordSalted = AddSaltToPassword(user, newPassword);
                passwordHashed = PasswordHasher.HashPassword(user, passwordSalted);
            }

            await passwordStore.SetPasswordHashAsync(user, passwordHashed, CancellationToken);
            await UpdateSecurityStampInternal(user);
            return IdentityResult.Success;
        }

        private string AddSaltToPassword(ApplicationUser user, string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            string newPassword = password;
            if (!string.IsNullOrEmpty(_encryptKey))
            {
                newPassword += _encryptKey;
            }

            if (!string.IsNullOrEmpty(user.PasswordSalt))
            {
                newPassword += user.PasswordSalt;
            }

            return newPassword;
        }

        // Update the security stamp if the store supports it
        private async Task UpdateSecurityStampInternal(ApplicationUser user)
        {
            if (SupportsUserSecurityStamp)
            {
                await GetSecurityStore().SetSecurityStampAsync(user, NewSecurityStamp(), CancellationToken);
            }
        }

        private string NewSecurityStamp()
        {
            byte[] bytes = new byte[20];
            _rng.GetBytes(bytes);
            return Base32.ToBase32(bytes);
        }

        private IUserPasswordStore<ApplicationUser> GetPasswordStore()
        {
            var cast = Store as IUserPasswordStore<ApplicationUser>;
            if (cast == null)
            {
                throw new NotSupportedException(ExceptionMessageConst.StoreNotIUserPasswordStore);
            }
            return cast;
        }

        private IUserSecurityStampStore<ApplicationUser> GetSecurityStore()
        {
            var cast = Store as IUserSecurityStampStore<ApplicationUser>;
            if (cast == null)
            {
                throw new NotSupportedException(ExceptionMessageConst.StoreNotIUserSecurityStampStore);
            }
            return cast;
        }
        #endregion
    }
}
