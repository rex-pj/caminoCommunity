using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using System.Threading.Tasks;
using System;

namespace Coco.Framework.SessionManager.Stores
{
    public class UserEmailStore : IUserEmailStore<ApplicationUser>
    {
        private bool _isDisposed;

        public async Task<string> GetEmailAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await Task.FromResult(user.Email);
        }

        public async Task<IApiResult> SendForgotPasswordAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await Task.FromResult(ApiResult.Success(user.ActiveUserStamp));
        }

        public virtual Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.IsEmailConfirmed);
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

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}
