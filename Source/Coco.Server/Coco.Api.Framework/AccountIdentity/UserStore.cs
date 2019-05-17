using Coco.Api.Framework.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Business.Contracts;
using Coco.Api.Framework.AccountIdentity.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using Coco.Entities.Model.Account;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Coco.Api.Framework.AccountIdentity
{
    public class UserStore : IUserStore<ApplicationUser>
    {
        private readonly IAccountBusiness _accountBusiness;
        private bool _isDisposed;
        /// <summary>
        /// A navigation property for the users the store contains.
        /// </summary>
        public IQueryable<ApplicationUser> Users
        {
            get;
        }


        public UserStore(IAccountBusiness accountBusiness)
        {
            _accountBusiness = accountBusiness;
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
        public virtual Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Users.SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
        }

        //public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        if (cancellationToken != null)
        //        {
        //            cancellationToken.ThrowIfCancellationRequested();
        //        }

        //        if (user == null)
        //        {
        //            throw new ArgumentNullException(nameof(user));
        //        }

        //        _accountBusiness.Delete(user.Id);

        //        return Task.FromResult(IdentityResult.Success);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        //    }
        //}

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

        //public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.NormalizedUserName);
        //}

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

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        //public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    user.UserName = userName;

        //    return Task.CompletedTask;
        //}

        //public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        if (cancellationToken != null)
        //        {
        //            cancellationToken.ThrowIfCancellationRequested();
        //        }

        //        if (user == null)
        //        {
        //            throw new ArgumentNullException(nameof(user));
        //        }

        //        var userModel = GetUserEntity(user);

        //        _accountBusiness.Update(userModel);

        //        return Task.FromResult(IdentityResult.Success);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        //    }
        //}

        //#endregion

        //#region IUserPasswordStore<LoggedUser> Members
        //public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    user.PasswordHash = passwordHash;

        //    return Task.CompletedTask;
        //}

        //public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.PasswordHash);
        //}

        //public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        //}
        //#endregion

        //#region IUserEmailStore<ApplicationUser> Members
        //public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    user.Email = email;

        //    return Task.CompletedTask;
        //}

        

        //public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.EmailConfirmed);
        //}

        //public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    user.EmailConfirmed = confirmed;

        //    return Task.CompletedTask;
        //}

        //public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        //{
        //    if (string.IsNullOrWhiteSpace(normalizedEmail))
        //    {
        //        throw new ArgumentNullException(nameof(normalizedEmail));
        //    }

        //    var userEntity = await _accountBusiness.FindUserByEmail(normalizedEmail, true);

        //    return await Task.FromResult(GetApplicationUser(userEntity));
        //}

        //public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        //{
        //    if (cancellationToken != null)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();
        //    }

        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return Task.FromResult(user.NormalizedEmail);
        //}


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

        private UserModel PopulateUserEntity(ApplicationUser LoggedUser)
        {
            UserModel userModel = new UserModel()
            {
                Email = LoggedUser.Email,
                Id = LoggedUser.Id,
                Address = LoggedUser.Address,
                BirthDate = LoggedUser.BirthDate,
                CountryId = LoggedUser.CountryId,
                CreatedById = LoggedUser.CreatedById,
                Description = LoggedUser.Description,
                DisplayName = LoggedUser.DisplayName,
                Firstname = LoggedUser.Firstname,
                Lastname = LoggedUser.Lastname,
                GenderId = LoggedUser.GenderId,
                IsActived = LoggedUser.IsActived,
                Password = LoggedUser.PasswordHash,
                PasswordSalt = LoggedUser.PasswordSalt,
                PhoneNumber = LoggedUser.PhoneNumber,
                StatusId = LoggedUser.StatusId,
                UpdatedById = LoggedUser.UpdatedById
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
                NormalizedUserName = userModel.Lastname + " " + userModel.Firstname,
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