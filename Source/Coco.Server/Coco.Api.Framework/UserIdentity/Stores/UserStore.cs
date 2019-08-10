using Coco.Api.Framework.Models;
using Coco.Api.Framework.UserIdentity.Contracts;
using Coco.Business.Contracts;
using Coco.Api.Framework.UserIdentity.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Coco.Entities.Model.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Coco.Entities.Model.General;
using AutoMapper;

namespace Coco.Api.Framework.UserIdentity.Stores
{
    public class UserStore : IUserStore<ApplicationUser>
    {
        private readonly IUserBusiness _userBusiness;
        private readonly ITextCrypter _textCrypter;
        private readonly string _textCrypterSaltKey;
        private readonly IMapper _mapper;

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.</value>
        public IdentityErrorDescriber Describer { get; private set; }
        private bool _isDisposed;

        public UserStore(IUserBusiness userBusiness,
            ITextCrypter textCrypter,
            IConfiguration configuration,
            IMapper mapper,
            IdentityErrorDescriber errors = null)
        {
            _textCrypterSaltKey = configuration["Crypter:SaltKey"];
            _userBusiness = userBusiness;
            _textCrypter = textCrypter;
            _mapper = mapper;
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

                var userModel = _mapper.Map<UserModel>(user);
                userModel.Password = user.PasswordHash;

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
            var result = _mapper.Map<ApplicationUser>(entity);
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
                var userModel = _mapper.Map<UserModel>(user);

                var result = await _userBusiness.UpdateAuthenticationAsync(userModel);
                string userIdentityId = _textCrypter.Encrypt(result.Id.ToString(), _textCrypterSaltKey);

                var userInfo = _mapper.Map<UserInfoModel>(user);
                userInfo.UserIdentityId = userIdentityId;
                return new ApiResult<UserTokenResult>(true)
                {
                    Result = new UserTokenResult()
                    {
                        AuthenticationToken = result.AuthenticationToken,
                        UserInfo = userInfo
                    }
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                return ApiResult.Failed(Describer.ConcurrencyFailure());
            }
        }

        public virtual async Task<ApiResult> UpdateUserProfileAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            try
            {
                var userModel = _mapper.Map<UserProfileUpdateModel>(user);
                var result = await _userBusiness.UpdateUserProfileAsync(userModel);

                return new ApiResult<UserProfileUpdateModel>(true)
                {
                    Result = new UserProfileUpdateModel()
                    {
                        AuthenticationToken = user.AuthenticationToken,
                        UserIdentityId = user.UserIdentityId,
                        DisplayName = result.DisplayName,
                        Firstname = result.Firstname,
                        Lastname = result.Lastname
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

        public async Task<ApplicationUser> FindByIdAsync(long userId, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var user = await _userBusiness.FindByIdAsync(userId);

            var result = _mapper.Map<ApplicationUser>(user);
            result.PasswordHash = user.Password;

            return await Task.FromResult(result);
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            var userEntity = await _userBusiness.FindUserByUsername(normalizedUserName.ToLower(), true);
            var result = _mapper.Map<ApplicationUser>(userEntity);
            result.UserName = result.Email;

            return await Task.FromResult(result);
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
        
            return _mapper.Map<ApplicationUser>(userEntity);
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