using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coco.Api.Framework.AccountIdentity
{
    public class AccountManager : IAccountManager<ApplicationUser>
    {
        #region Fields
        private bool _isDisposed;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IEnumerable<IUserValidator<ApplicationUser>> _userValidators;
        #endregion

        #region Ctor
        public AccountManager(IUserStore<ApplicationUser> userStore, IEnumerable<IUserValidator<ApplicationUser>> userValidators)
        {
            _userStore = userStore;
            _userValidators = userValidators;
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
        //public virtual async Task<IdentityResult> CreateAsync(ApplicationUser user)
        //{
        //    ThrowIfDisposed();
        //    var result = await ValidateUserAsync(user);
        //    if (!result.IsSuccess)
        //    {
        //        return result;
        //    }
        //    //await UpdateNormalizedUserNameAsync(user);
        //    //await UpdateNormalizedEmailAsync(user);

        //    return await Store.CreateAsync(user, CancellationToken);
        //}

        ///// <summary>
        ///// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        ///// called before saving the user via Create or Update.
        ///// </summary>
        ///// <param name="user">The user</param>
        ///// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        //protected async Task<IdentityResult> ValidateUserAsync(ApplicationUser user)
        //{
        //    var errors = new List<IdentityError>();
        //    foreach (var v in _userValidators)
        //    {
        //        var result = await v.ValidateAsync(this, user);
        //        if (!result.Succeeded)
        //        {
        //            errors.AddRange(result.Errors);
        //        }
        //    }
        //    if (errors.Count > 0)
        //    {
        //        return IdentityResult.Failed(errors.ToArray());
        //    }
        //    return new IdentityResult(true);
        //}
        #endregion

        #region Privates
        
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
