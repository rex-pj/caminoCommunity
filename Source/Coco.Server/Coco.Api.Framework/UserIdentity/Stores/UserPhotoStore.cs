using Coco.Api.Framework.Models;
using Coco.Api.Framework.UserIdentity.Contracts;
using Coco.Business.Contracts;
using Coco.Api.Framework.UserIdentity.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.EntityFrameworkCore;
using Coco.Entities.Model.General;
using Coco.Entities.Enums;
using Coco.Common.Exceptions;

namespace Coco.Api.Framework.UserIdentity.Stores
{
    public class UserPhotoStore : IUserPhotoStore<ApplicationUser>
    {
        private readonly IUserPhotoBusiness _userPhotoBusiness;

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.</value>
        public IdentityErrorDescriber Describer { get; private set; }
        private bool _isDisposed;

        public UserPhotoStore(IUserPhotoBusiness userPhotoBusiness,
            IdentityErrorDescriber errors = null)
        {
            _userPhotoBusiness = userPhotoBusiness;
            Describer = errors ?? new IdentityErrorDescriber();
        }


        /// <summary>
        /// Updates photo <paramref name="model"/> in the user store.
        /// </summary>
        /// <param name="model">The photo to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        public virtual async Task<ApiResult> UpdateAvatarAsync(UpdateUserPhotoModel model, long userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            try
            {
                model.UserPhotoCode = Guid.NewGuid().ToString();
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(model, userId);

                return ApiResult<UpdateUserPhotoModel>.Success(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                return ApiResult.Failed(Describer.ConcurrencyFailure());
            }
            catch(PhotoSizeInvalidException)
            {
                return ApiResult.Failed(Describer.PhotoSizeInvalid());
            }
        }

        /// <summary>
        /// Updates photo <paramref name="model"/> in the user store.
        /// </summary>
        /// <param name="model">The photo to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        public virtual async Task<ApiResult> UpdateCoverAsync(UpdateUserPhotoModel model, long userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            try
            {
                model.UserPhotoCode = Guid.NewGuid().ToString();
                var result = await _userPhotoBusiness.UpdateUserPhotoAsync(model, userId);

                return ApiResult<UpdateUserPhotoModel>.Success(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                return ApiResult.Failed(Describer.ConcurrencyFailure());
            }
        }

        /// <summary>
        /// Updates photo <paramref name="model"/> in the user store.
        /// </summary>
        /// <param name="model">The photo to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        public virtual async Task<ApiResult> DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                await _userPhotoBusiness.DeleteUserPhotoAsync(userId, userPhotoType);

                return ApiResult<UpdateUserPhotoModel>.Success(new UpdateUserPhotoModel());
            }
            catch (DbUpdateConcurrencyException)
            {
                return ApiResult.Failed(Describer.ConcurrencyFailure());
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

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}