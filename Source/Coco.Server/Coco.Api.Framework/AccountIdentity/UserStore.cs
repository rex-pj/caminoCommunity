using Coco.Api.Framework.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Business.Contracts;
using Coco.Api.Framework.AccountIdentity.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Coco.Entities.Model.Account;
using Microsoft.EntityFrameworkCore;

namespace Coco.Api.Framework.AccountIdentity
{
    public class UserStore : IUserStore<ApplicationUser>
    {
        private readonly IAccountBusiness _accountBusiness;
        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.</value>
        public IdentityErrorDescriber Describer { get; private set; }
        private bool _isDisposed;

        public UserStore(IAccountBusiness accountBusiness,
            IdentityErrorDescriber errors = null)
        {
            _accountBusiness = accountBusiness;
            Describer = errors ?? new IdentityErrorDescriber();
        }

        #region IUserStore<LoggedUser> Members
        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
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

                _accountBusiness.Add(userModel);

                return Task.FromResult(new IdentityResult(true));
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
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

            var entity = await _accountBusiness.FindUserByEmail(normalizedEmail);
            var result = GetLoggedUser(entity);
            return result;
        }

        /// <summary>
        /// Updates the specified <paramref name="user"/> in the user store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        public virtual async Task<LoginResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
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

                var result = await _accountBusiness.UpdateAsync(userModel);

                return new LoginResult(true) {
                    AuthenticatorToken = result.AuthenticatorToken,
                    Expiration = result.Expiration
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                return LoginResult.Failed(Describer.ConcurrencyFailure());
            }
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
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

                _accountBusiness.Delete(user.Id);

                return Task.FromResult(new IdentityResult(true));
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
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

            var userEntity = await _accountBusiness.Find(id);

            return await Task.FromResult(GetLoggedUser(userEntity));
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            var userEntity = await _accountBusiness.FindUserByUsername(normalizedUserName.ToLower(), true);

            return await Task.FromResult(GetLoggedUser(userEntity));
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

        #region Private Methods
        private UserModel GetUserEntity(ApplicationUser LoggedUser)
        {
            if (LoggedUser == null)
            {
                return null;
            }

            var result = PopulateUserEntity(LoggedUser);

            return result;
        }

        private ApplicationUser GetApplicationUser(UserModel entity)
        {
            if (entity == null)
            {
                return null;
            }

            var result = PopulateLoggedUser(entity);

            return result;
        }

        private UserModel PopulateUserEntity(ApplicationUser loggedUser)
        {
            UserModel userModel = new UserModel()
            {
                Email = loggedUser.Email,
                Id = loggedUser.Id,
                Address = loggedUser.Address,
                BirthDate = loggedUser.BirthDate,
                CountryId = loggedUser.CountryId,
                CreatedById = loggedUser.CreatedById,
                Description = loggedUser.Description,
                DisplayName = loggedUser.DisplayName,
                Firstname = loggedUser.Firstname,
                Lastname = loggedUser.Lastname,
                GenderId = loggedUser.GenderId,
                IsActived = loggedUser.IsActived,
                Password = loggedUser.PasswordHash,
                PasswordSalt = loggedUser.PasswordSalt,
                PhoneNumber = loggedUser.PhoneNumber,
                StatusId = loggedUser.StatusId,
                UpdatedById = loggedUser.UpdatedById,
                Expiration = loggedUser.Expiration,
                AuthenticatorToken = loggedUser.AuthenticatorToken,
                SecurityStamp = loggedUser.SecurityStamp
            };

            return userModel;
        }

        private ApplicationUser GetLoggedUser(UserModel entity)
        {
            if (entity == null)
            {
                return null;
            }

            var result = PopulateLoggedUser(entity);

            return result;
        }

        private ApplicationUser PopulateLoggedUser(UserModel userModel)
        {
            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = userModel.Email,
                Id = userModel.Id,
                UserName = userModel.Email,
                Lastname = userModel.Lastname,
                Firstname = userModel.Firstname,
                Password = userModel.Password,
                PasswordHash = userModel.Password,
                PasswordSalt = userModel.PasswordSalt,
                PhoneNumber = userModel.PhoneNumber,
                Address = userModel.Address,
                BirthDate = userModel.BirthDate,
                CountryId = userModel.CountryId,
                CreatedById = userModel.CreatedById,
                Description = userModel.Description,
                DisplayName = userModel.DisplayName,
                GenderId = userModel.GenderId,
                IsActived = userModel.IsActived,
                StatusId = userModel.StatusId,
                UpdatedById = userModel.UpdatedById,
                Expiration = userModel.Expiration,
                AuthenticatorToken = userModel.AuthenticatorToken,
                SecurityStamp = userModel.SecurityStamp
            };

            return applicationUser;
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