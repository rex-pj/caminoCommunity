using Coco.Api.Framework.Commons.Encode;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.SessionManager.Contracts;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Coco.Api.Framework.SessionManager.Providers
{
    public abstract class TimeStampTokenProvider<TUser> : IUserTokenProvider<TUser> where TUser : ApplicationUser
    {
        public abstract Task<bool> CanGenerateTwoFactorTokenAsync(IUserManager<TUser> manager, TUser user);

        public virtual async Task<string> GenerateAsync(string purpose, IUserManager<TUser> manager, TUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            var token = await manager.CreateSecurityTokenAsync(user);
            var modifier = await GetUserModifierAsync(purpose, manager, user);
            return Rfc6238AuthenticationEncoder.GenerateCode(token, modifier).ToString("D6", CultureInfo.InvariantCulture);
        }

        public virtual async Task<string> GetUserModifierAsync(string purpose, IUserManager<TUser> manager, TUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            var userId = await manager.GetUserIdAsync(user);
            return "Totp:" + purpose + ":" + userId;
        }

        public virtual async Task<bool> ValidateAsync(string purpose, string token, IUserManager<TUser> manager, TUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            int code;
            if (!int.TryParse(token, out code))
            {
                return false;
            }
            var securityToken = await manager.CreateSecurityTokenAsync(user);
            var modifier = await GetUserModifierAsync(purpose, manager, user);
            return securityToken != null && Rfc6238AuthenticationEncoder.ValidateCode(securityToken, code, modifier);
        }
    }
}
