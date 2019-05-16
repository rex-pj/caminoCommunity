using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        protected internal IUserEmailStore<ApplicationUser> UserEmailStore;
        public IdentityOptions Options { get; set; }
        /// <summary>
        /// The cancellation token used to cancel operations.
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;
        #endregion

        #region Fields
        private bool _isDisposed;
        private readonly IEnumerable<IUserValidator<ApplicationUser>> _userValidators;

        private readonly ILookupNormalizer _lookupNormalizer;
        #endregion

        #region Ctor
        public AccountManager(IUserStore<ApplicationUser> userStore,
            IUserEmailStore<ApplicationUser> userEmailStore,
            IOptions<IdentityOptions> optionsAccessor,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            ILookupNormalizer lookupNormalizer)
        {
            this.Options = optionsAccessor?.Value ?? new IdentityOptions();
            this.UserStore = userStore;
            this.UserEmailStore = userEmailStore;
            _userValidators = userValidators;
            _lookupNormalizer = lookupNormalizer;
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
            var result = await ValidateUserAsync(user);
            if (!result.IsSuccess)
            {
                return result;
            }

            await UpdateNormalizedUserNameAsync(user);
            await UpdateNormalizedEmailAsync(user);

            return await UserStore.CreateAsync(user, CancellationToken);
        }

        /// <summary>
        /// Updates the normalized user name for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose user name should be normalized and updated.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public virtual async Task UpdateNormalizedUserNameAsync(ApplicationUser user)
        {
            var normalizedName = NormalizeName(await GetUserNameAsync(user));
            normalizedName = ProtectPersonalData(normalizedName);
            await UserStore.SetNormalizedUserNameAsync(user, normalizedName, CancellationToken);
        }

        /// <summary>
        /// Updates the normalized email for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose email address should be normalized and updated.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task UpdateNormalizedEmailAsync(ApplicationUser user)
        {
            var email = await GetEmailAsync(user);
            await UserEmailStore.SetNormalizedEmailAsync(user, ProtectPersonalData(NormalizeEmail(email)), CancellationToken);
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

        ///// <summary>
        ///// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        ///// called before saving the user via Create or Update.
        ///// </summary>
        ///// <param name="user">The user</param>
        ///// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        protected virtual async Task<IdentityResult> ValidateUserAsync(ApplicationUser user)
        {
            var errors = new List<IdentityError>();
            foreach (var v in _userValidators)
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

        #endregion

        #region Privates
        private string ProtectPersonalData(string data)
        {
            //if (Options.Stores.ProtectPersonalData)
            //{
            //    var keyRing = _services.GetService<ILookupProtectorKeyRing>();
            //    var protector = _services.GetService<ILookupProtector>();
            //    return protector.Protect(keyRing.CurrentKeyId, data);
            //}
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
