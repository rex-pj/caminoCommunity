using Coco.Api.Framework.Models;
using Coco.Api.Framework.SessionManager.Contracts;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Net.Mail;

namespace Coco.Api.Framework.SessionManager.Stores
{
    public class UserEmailStore : IUserEmailStore<ApplicationUser>
    {
        private bool _isDisposed;

        public async Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await Task.FromResult(user.Email);
        }

        public async Task<ApiResult> SendForgotPasswordAsync(string email, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await Task.FromResult(ApiResult.Success());
        }

        public virtual Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
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
