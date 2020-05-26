using AutoMapper;
using Coco.Framework.Models;
using Coco.Framework.SessionManager.Core;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.User;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coco.Framework.SessionManager.Stores.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Coco.Framework.SessionManager.Stores
{
    public class UserAttributeStore<TUser> : IUserAttributeStore<TUser> where TUser : IdentityUser<long>
    {
        private readonly IUserAttributeBusiness _userAttributeBusiness;
        private readonly IMapper _mapper;
        private readonly int _resetPasswordExpirationHours;
        public readonly int _registerConfimationExpirationHours;

        public UserAttributeStore(IUserAttributeBusiness userAttributeBusiness, IMapper mapper, IConfiguration configuration)
        {
            _userAttributeBusiness = userAttributeBusiness;
            _mapper = mapper;
            var resetPasswordExpirationHours = configuration["ResetPassword:ExpirationHours"];
            int.TryParse(resetPasswordExpirationHours, out _resetPasswordExpirationHours);
            var registerConfimationExpirationHours = configuration["RegisterConfimation:ExpirationHours"];
            int.TryParse(registerConfimationExpirationHours, out _registerConfimationExpirationHours);
        }

        public IEnumerable<UserAttributeDto> GetUserAttributes(long userId)
        {
            var data = _userAttributeBusiness.Get(userId);
            return _mapper.Map<IEnumerable<UserAttributeDto>>(data);
        }

        public ApplicationUserToken GetAuthenticationAttribute(long userId, string tokenName)
        {
            var result = new ApplicationUserToken();
            if (string.IsNullOrEmpty(tokenName))
            {
                return result;
            }

            var attributes = GetUserAttributes(userId);
            if (attributes == null || !attributes.Any())
            {
                return result;
            }

            var tokenResult = attributes
                .FirstOrDefault(x => x.Key == UserAttributeOptions.AUTHENTICATION_TOKEN && x.Value.Equals(tokenName)
                && x.Expiration > DateTime.UtcNow && !x.IsDisabled);

            if (tokenResult != null)
            {
                result.Value = tokenResult.Value;
            }

            return result;
        }

        public async Task<string> GetSecurityStampAsync(long userId, string key)
        {
            var data = await _userAttributeBusiness.GetAsync(userId, key);
            if (data == null)
            {
                return null;
            }

            return data.Value;
        }

        public async Task<string> GetSecurityStampAsync(long userId)
        {
            return await GetSecurityStampAsync(userId, UserAttributeOptions.SECURITY_STAMP);
        }

        public async Task<UserAttributeDto> SetResetPasswordStampAsync(ApplicationUser user, string stamp)
        {
            var expiration = DateTime.UtcNow.AddHours(_resetPasswordExpirationHours);
            var data = await _userAttributeBusiness.CreateOrUpdateAsync(user.Id, UserAttributeOptions.RESET_PASSWORD_BY_EMAIL_CONFIRM, stamp, expiration);
            var result = _mapper.Map<UserAttributeDto>(data);
            return result;
        }

        public async Task<IEnumerable<UserAttributeDto>> SetAttributesAsync(IEnumerable<UserAttributeDto> userAttributes)
        {
            var data = await _userAttributeBusiness.CreateOrUpdateAsync(userAttributes);
            var result = _mapper.Map<IEnumerable<UserAttributeDto>>(data);

            return result;
        }

        public IEnumerable<UserAttributeDto> NewUserRegisterAttributes(ApplicationUser user)
        {
            yield return new UserAttributeDto()
            {
                UserId = user.Id,
                Key = UserAttributeOptions.SECURITY_STAMP,
                Value = !string.IsNullOrEmpty(user.SecurityStamp) ? user.SecurityStamp : NewSecurityStamp()
            };
        }

        public IEnumerable<UserAttributeDto> NewUserAuthenticationAttributes(ApplicationUser user)
        {
            yield return new UserAttributeDto()
            {
                UserId = user.Id,
                Key = UserAttributeOptions.AUTHENTICATION_TOKEN,
                Value = user.AuthenticationToken,
                Expiration = user.Expiration
            };
        }

        public async Task<bool> DeleteUserAuthenticationAttributes(ApplicationUser user, string authenticationToken)
        {
            var isSucceed = await _userAttributeBusiness.DeleteAsync(user.Id, UserAttributeOptions.AUTHENTICATION_TOKEN, authenticationToken);
            return isSucceed;
        }

        public async Task<bool> DeleteUserActivationAttribute(ApplicationUser user)
        {
            var isSucceed = await DeleteUserAttribute(user, UserAttributeOptions.ACTIVE_USER_BY_EMAIL_CONFIRM);
            return isSucceed;
        }

        public async Task<bool> DeleteResetPasswordByEmailAttribute(ApplicationUser user)
        {
            var isSucceed = await DeleteUserAttribute(user, UserAttributeOptions.RESET_PASSWORD_BY_EMAIL_CONFIRM);
            return isSucceed;
        }

        public async Task<bool> DeleteUserAttribute(ApplicationUser user, string key)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var isSucceed = await _userAttributeBusiness.DeleteAsync(user.Id, key);
            return isSucceed;
        }

        public string NewSecurityStamp()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
