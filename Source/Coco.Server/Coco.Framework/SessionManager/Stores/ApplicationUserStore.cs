using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.User;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Stores.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Stores
{
    public class ApplicationUserStore : IUserStore<ApplicationUser>
    {
        private readonly IUserBusiness _userBusiness;
        private IMapper _mapper;
        private bool _disposed;
        private readonly IUserAttributeStore<ApplicationUser> _userAttributeStore;

        public ApplicationUserStore(IUserBusiness userBusiness, IUserAttributeStore<ApplicationUser> userAttributeStore, IMapper mapper)
        {
            _userBusiness = userBusiness;
            _userAttributeStore = userAttributeStore;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Password = user.PasswordHash;

                var response = await _userBusiness.CreateAsync(userDto);
                if (response.Id > 0)
                {
                    var userResponse = _mapper.Map<ApplicationUser>(response);
                    userResponse.ActiveUserStamp = user.ActiveUserStamp;
                    userResponse.SecurityStamp = user.SecurityStamp;

                    var newUserAttributes = _userAttributeStore.NewUserRegisterAttributes(userResponse);
                    await _userAttributeStore.SetAttributesAsync(newUserAttributes);
                }

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed();
            }
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var id = long.Parse(userId);
            var user = await _userBusiness.FindByIdAsync(id);
            return _mapper.Map<ApplicationUser>(user);
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = await _userBusiness.FindByUsernameAsync(normalizedUserName);
            return _mapper.Map<ApplicationUser>(user);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(ConvertIdToString(user.Id));
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual string ConvertIdToString(long id)
        {
            if (object.Equals(id, default(long)))
            {
                return null;
            }
            return id.ToString();
        }

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }
}