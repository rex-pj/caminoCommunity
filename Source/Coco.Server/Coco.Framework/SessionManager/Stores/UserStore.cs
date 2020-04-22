using Coco.Framework.Models;
using Coco.Framework.SessionManager.Contracts;
using Coco.Business.Contracts;
using System.Threading.Tasks;
using System;
using Coco.Entities.Dtos.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Coco.Entities.Dtos.General;
using AutoMapper;
using Coco.Framework.SessionManager.Core;
using System.Linq;
using Coco.Common.Exceptions;
using Coco.Commons.Models;

namespace Coco.Framework.SessionManager.Stores
{
    public class UserStore : IUserStore<ApplicationUser>
    {
        private readonly IUserBusiness _userBusiness;
        private readonly ITextCrypter _textCrypter;
        private readonly string _textCrypterSaltKey;
        private readonly IMapper _mapper;
        private readonly IUserAttributeBusiness _userAttributeBusiness;
        private readonly IUserStampStore<ApplicationUser> _userStampStore;

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.</value>
        public IdentityErrorDescriber Describer { get; private set; }
        private bool _isDisposed;

        public UserStore(IUserBusiness userBusiness,
            IUserAttributeBusiness userAttributeBusiness,
            ITextCrypter textCrypter,
            IConfiguration configuration,
            IMapper mapper,
            IUserStampStore<ApplicationUser> userStampStore,
            IdentityErrorDescriber errors = null)
        {
            _textCrypterSaltKey = configuration["Crypter:SaltKey"];
            _userBusiness = userBusiness;
            _userAttributeBusiness = userAttributeBusiness;
            _textCrypter = textCrypter;
            _mapper = mapper;
            _userStampStore = userStampStore;
            Describer = errors ?? new IdentityErrorDescriber();
        }

        #region IUserStore<LoggedUser> Members
        public async Task<IApiResult> CreateAsync(ApplicationUser user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                var userModel = _mapper.Map<UserDto>(user);
                userModel.Password = user.PasswordHash;

                var userData = await _userBusiness.CreateAsync(userModel);
                var userResult = _mapper.Map<ApplicationUser>(userData);

                return ApiResult.Success(userResult);
            }
            catch (Exception ex)
            {
                return ApiResult.Failed(new CommonError { Code = ex.Message, Message = ex.Message });
            }
        }
        #endregion

        /// <summary>
        /// Gets the user, if any, associated with the specified, normalized email address.
        /// </summary>
        /// <param name="normalizedEmail">The normalized email address to return the user for.</param>
        /// <returns>
        /// The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized email address.
        /// </returns>
        public virtual async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail)
        {
            ThrowIfDisposed();

            var entity = await _userBusiness.FindUserByEmail(normalizedEmail);
            var result = _mapper.Map<ApplicationUser>(entity);
            return result;
        }

        /// <summary>
        /// Updates the specified <paramref name="user"/> in the user store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        public virtual async Task<IApiResult> UpdateAuthenticationAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                var userModel = _mapper.Map<UserDto>(user);
                var authenticationAttributes = _userStampStore.NewUserAuthenticationAttributes(user);
                var userAttribute = await _userAttributeBusiness.CreateAsync(authenticationAttributes);

                if (userAttribute == null || !userAttribute.Any())
                {
                    return ApiResult.Failed(Describer.InvalidToken());
                }

                var authTokenResult = userAttribute.FirstOrDefault(x => x.Key == UserAttributeOptions.AUTHENTICATION_TOKEN);
                if (authTokenResult == null || string.IsNullOrEmpty(authTokenResult.Value))
                {
                    throw new UnauthorizedAccessException();
                }

                string userIdentityId = _textCrypter.Encrypt(authTokenResult.UserId.ToString(), _textCrypterSaltKey);
                var userInfo = _mapper.Map<UserInfoModel>(user);
                userInfo.UserIdentityId = userIdentityId;
                return ApiResult.Success(new UserTokenResult()
                {
                    AuthenticationToken = authTokenResult.Value,
                    UserInfo = userInfo
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return ApiResult.Failed(Describer.ConcurrencyFailure());
            }
        }

        public virtual async Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(ApplicationUser user)
        {
            ThrowIfDisposed();

            try
            {
                var userModel = _mapper.Map<UserIdentifierUpdateDto>(user);
                var result = await _userBusiness.UpdateIdentifierAsync(userModel);

                return new UserIdentifierUpdateDto()
                {
                    DisplayName = result.DisplayName,
                    Firstname = result.Firstname,
                    Lastname = result.Lastname
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new CocoApplicationException(Describer.ConcurrencyFailure());
            }
        }

        public async Task<ApplicationUser> FindByIdAsync(long userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var user = await _userBusiness.FindByIdAsync(userId);

            var result = _mapper.Map<ApplicationUser>(user);
            result.PasswordHash = user.Password;

            return result;
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName)
        {
            var userEntity = await _userBusiness.FindUserByUsername(normalizedUserName.ToLower());
            var result = _mapper.Map<ApplicationUser>(userEntity);

            if (result != null)
            {
                result.UserName = result.Email;
            }

            return result;
        }

        public ApplicationUser FindByIdentityId(string userIdentityId)
        {
            var userIdDecrypted = _textCrypter.Decrypt(userIdentityId, _textCrypterSaltKey);
            var userId = long.Parse(userIdDecrypted);

            var user = GetLoggedInUser(userId);
            return user;
        }

        private ApplicationUser GetLoggedInUser(long id)
        {
            var userEntity = _userBusiness.GetLoggedIn(id);
            return _mapper.Map<ApplicationUser>(userEntity);
        }

        public async Task<UserFullDto> FindByIdentityIdAsync(string userIdentityId)
        {
            var userIdDecrypted = _textCrypter.Decrypt(userIdentityId, _textCrypterSaltKey);
            var userId = long.Parse(userIdDecrypted);

            var user = await FindFullByIdAsync(userId);
            return await Task.FromResult(user);
        }

        public async Task<UserFullDto> FindFullByIdAsync(long id)
        {
            var userEntity = await _userBusiness.FindFullByIdAsync(id);

            return await Task.FromResult(userEntity);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Id.ToString());
        }

        public string GetUserNameAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.UserName;
        }

        /// <summary>
        /// Updates the specified <paramref name="model"/> in the user store.
        /// </summary>
        /// <param name="model">The user info to update.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        public virtual async Task<UpdatePerItemModel> UpdateInfoItemAsync(UpdatePerItemModel model)
        {
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
                var userModel = new UpdatePerItemDto()
                {
                    Key = userId,
                    Value = model.Value,
                    PropertyName = model.PropertyName
                };

                var result = await _userBusiness.UpdateInfoItemAsync(userModel);

                model.Value = result.Value;

                return model;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new CocoApplicationException(ex);
            }
        }

        public async Task<IApiResult> ActiveAsync(ApplicationUser user)
        {
            try
            {
                var isSucceed = await _userBusiness.ActiveAsync(user.Id);

                return new ApiResult(isSucceed);
            }
            catch (Exception ex)
            {
                return ApiResult.Failed(new CommonError { Code = ex.Message, Message = ex.Message });
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