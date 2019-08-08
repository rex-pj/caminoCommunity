using Coco.Api.Framework.Models;
using Coco.Api.Framework.UserIdentity.Contracts;
using System.Threading.Tasks;
using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Coco.Business.Contracts;
using Coco.Entities.Model.User;

namespace Coco.Api.Framework.UserIdentity.Stores
{
    public class UserPasswordStore : IUserPasswordStore<ApplicationUser>
    {
        private bool _isDisposed;
        internal readonly string _encryptKey;
        private readonly IUserBusiness _userBusiness;

        public UserPasswordStore(IConfiguration configuration, IUserBusiness userBusiness)
        {
            _encryptKey = configuration.GetValue<string>("EncryptKey");
            _userBusiness = userBusiness;
        }

        /// <summary>
        /// Sets the password hash for a user.
        /// </summary>
        /// <param name="user">The user to set the password hash for.</param>
        /// <param name="passwordHash">The password hash to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public virtual Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public string AddSaltToPassword(ApplicationUser user, string password)
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

        /// <summary>
        /// Gets the password hash for a user.
        /// </summary>
        /// <param name="user">The user to retrieve the password hash for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the password hash for the user.</returns>
        public virtual Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// Changes a user's password after confirming the specified <paramref name="currentPassword"/> is correct,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to retrieve the password hash for.</param>
        /// <param name="currentPassword">The current password of the user.</param>
        /// <param name="newPassword">The password that I want to update to.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the password hash for the user.</returns>
        public virtual async Task<bool> ChangePasswordAsync(long userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var model = new UserPasswordUpdateModel()
            {
                UserId = userId,
                NewPassword = newPassword,
                CurrentPassword = currentPassword,
            };

            return await _userBusiness.UpdatePasswordAsync(model);
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
