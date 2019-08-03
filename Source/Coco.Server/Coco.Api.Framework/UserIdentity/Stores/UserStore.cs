using Coco.Api.Framework.Models;
using Coco.Api.Framework.UserIdentity.Contracts;
using Coco.Business.Contracts;
using Coco.Api.Framework.UserIdentity.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Coco.Entities.Model.User;
using Microsoft.EntityFrameworkCore;
using Coco.Api.Framework.Mapping;
using Microsoft.Extensions.Configuration;
using Coco.Entities.Model.General;
using Coco.Entities.Enums;

namespace Coco.Api.Framework.UserIdentity.Stores
{
    public class UserStore : IUserStore<ApplicationUser>
    {
        private readonly IUserBusiness _userBusiness;
        private readonly ITextCrypter _textCrypter;
        private readonly string _textCrypterSaltKey;

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.</value>
        public IdentityErrorDescriber Describer { get; private set; }
        private bool _isDisposed;

        public UserStore(IUserBusiness userBusiness,
            ITextCrypter textCrypter,
            IConfiguration configuration,
            IdentityErrorDescriber errors = null)
        {
            _textCrypterSaltKey = configuration["Crypter:SaltKey"];
            _userBusiness = userBusiness;
            _textCrypter = textCrypter;
            Describer = errors ?? new IdentityErrorDescriber();
        }

        #region IUserStore<LoggedUser> Members
        public Task<ApiResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken != null)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                var userModel = GetUserEntity(user);

                _userBusiness.Add(userModel);

                return Task.FromResult(new ApiResult(true));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ApiResult.Failed(new ApiError { Code = ex.Message, Description = ex.Message }));
            }
        }
        #endregion

        /// <summary>
        /// Gets the user, if any, associated with the specified, normalized email address.
        /// </summary>
        /// <param name="normalizedEmail">The normalized email address to return the user for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized email address.
        /// </returns>
        public virtual async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var entity = await _userBusiness.FindUserByEmail(normalizedEmail);
            var result = GetApplicationUser(entity);
            return result;
        }

        /// <summary>
        /// Updates the specified <paramref name="user"/> in the user store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        public virtual async Task<ApiResult> UpdateAuthenticationAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                var userModel = GetUserEntity(user);

                var result = await _userBusiness.UpdateAuthenticationAsync(userModel);
                string userIdentityId = _textCrypter.Encrypt(result.Id.ToString(), _textCrypterSaltKey);

                return new ApiResult<LoginResult>(true)
                {
                    Result = new LoginResult()
                    {
                        AuthenticationToken = result.AuthenticationToken,
                        Expiration = result.Expiration,
                        UserInfo = UserInfoMapping.ApplicationUserToUserInfo(user, userIdentityId)
                    }
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                return ApiResult.Failed(Describer.ConcurrencyFailure());
            }
        }

        public Task<ApiResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken != null)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                _userBusiness.Delete(user.Id);

                return Task.FromResult(new ApiResult(true));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ApiResult.Failed(new ApiError { Code = ex.Message, Description = ex.Message }));
            }
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (!long.TryParse(userId, out long id))
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"{nameof(userId)} is not a valid GUID");
            }

            var userEntity = await _userBusiness.FindByIdAsync(id);

            return await Task.FromResult(GetApplicationUser(userEntity));
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            var userEntity = await _userBusiness.FindUserByUsername(normalizedUserName.ToLower(), true);

            return await Task.FromResult(GetApplicationUser(userEntity));
        }

        public ApplicationUser FindByIdentityId(string userIdentityId, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            var userIdDecrypted = _textCrypter.Decrypt(userIdentityId, _textCrypterSaltKey);
            var userId = long.Parse(userIdDecrypted);

            var user = GetLoggedInUser(userId, cancellationToken);
            return user;
        }

        private ApplicationUser GetLoggedInUser(long id, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            var userEntity = _userBusiness.GetLoggedIn(id);

            return GetLoggedInUser(userEntity);
        }

        public async Task<UserFullModel> GetFullByFindByHashedIdAsync(string userIdentityId, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            var userIdDecrypted = _textCrypter.Decrypt(userIdentityId, _textCrypterSaltKey);
            var userId = long.Parse(userIdDecrypted);

            var user = await GetFullByIdAsync(userId, cancellationToken);
            return await Task.FromResult(user);
        }

        public async Task<UserFullModel> GetFullByIdAsync(long id, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            var userEntity = await _userBusiness.GetFullByIdAsync(id);

            return await Task.FromResult(userEntity);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.UserName);
        }

        /// <summary>
        /// Updates the specified <paramref name="model"/> in the user store.
        /// </summary>
        /// <param name="model">The user info to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        public virtual async Task<ApiResult> UpdateInfoItemAsync(UpdatePerItemModel model, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.PropertyName == null)
            {
                throw new ArgumentNullException(nameof(model.PropertyName));
            }

            if (model.Key == null)
            {
                throw new ArgumentNullException(nameof(model.Key));
            }

            try
            {
                var userIdDecrypted = _textCrypter.Decrypt(model.Key.ToString(), _textCrypterSaltKey);
                var userId = long.Parse(userIdDecrypted);
                var userModel = new UpdatePerItem()
                {
                    Key = userId,
                    Value = model.Value,
                    PropertyName = model.PropertyName
                };

                var result = await _userBusiness.UpdateInfoItemAsync(userModel);

                model.Value = result.Value;

                return ApiResult<UpdatePerItemModel>.Success(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return ApiResult.Failed(Describer.ConcurrencyFailure());
            }
        }

        #region Private Methods
        private UserModel GetUserEntity(ApplicationUser loggedUser)
        {
            if (loggedUser == null)
            {
                return null;
            }

            var result = UserInfoMapping.PopulateUserEntity(loggedUser);

            return result;
        }

        private ApplicationUser GetApplicationUser(UserModel entity)
        {
            if (entity == null)
            {
                return null;
            }

            var result = UserInfoMapping.PopulateApplicationUser(entity);

            return result;
        }

        private ApplicationUser GetLoggedInUser(UserLoggedInModel model)
        {
            if (model == null)
            {
                return null;
            }

            var result = UserInfoMapping.PopulateLoggedInUser(model);

            return result;
        }
        #endregion

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