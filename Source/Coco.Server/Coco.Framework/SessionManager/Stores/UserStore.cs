using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.User;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Stores.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Stores
{
    public class UserStore : IUserStore<ApplicationUser>
    {
        private readonly IUserBusiness _userBusiness;
        private IMapper _mapper;
        private readonly IUserAttributeStore<ApplicationUser> _userAttributeStore;

        public UserStore(IUserBusiness userBusiness, IUserAttributeStore<ApplicationUser> userAttributeStore, IMapper mapper)
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

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}