using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager
{
    public class ApplicationUserManager<TUser> : UserManager<TUser>, IDisposable where TUser: IdentityUser<long>
    {
        public ApplicationUserManager(IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services,
                 logger)
        {
        }

        //public virtual async Task<IdentityResult> CreateAsync(TUser user)
        //{
        //    ThrowIfDisposed();
        //    await UpdateSecurityStampInternal(user);
        //    var result = await ValidateUserAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        return result;
        //    }
        //    if (Options.Lockout.AllowedForNewUsers && SupportsUserLockout)
        //    {
        //        await GetUserLockoutStore().SetLockoutEnabledAsync(user, true, CancellationToken);
        //    }
        //    await UpdateNormalizedUserNameAsync(user);
        //    await UpdateNormalizedEmailAsync(user);

        //    return await Store.CreateAsync(user, CancellationToken);
        //}

//        public virtual async Task UpdateNormalizedUserNameAsync(TUser user)
//        {
//            var normalizedName = NormalizeName(await GetUserNameAsync(user));
//            normalizedName = ProtectPersonalData(normalizedName);
//            await Store.SetNormalizedUserNameAsync(user, normalizedName, CancellationToken);
//        }

//        private string ProtectPersonalData(string data)
//        {
//            if (Options.Stores.ProtectPersonalData)
//            {
//                var keyRing = _services.GetService<ILookupProtectorKeyRing>();
//                var protector = _services.GetService<ILookupProtector>();
//                return protector.Protect(keyRing.CurrentKeyId, data);
//            }
//            return data;
//        }

//        private async Task UpdateSecurityStampInternal(TUser user)
//        {
//            if (SupportsUserSecurityStamp)
//            {
//                await GetSecurityStore().SetSecurityStampAsync(user, NewSecurityStamp(), CancellationToken);
//            }
//        }

//        private static string NewSecurityStamp()
//        {
//            byte[] bytes = new byte[20];
//#if NETSTANDARD2_0
//            _rng.GetBytes(bytes);
//#else
//            RandomNumberGenerator.Fill(bytes);
//#endif
//            return Base32.ToBase32(bytes);
//        }

//        private IUserSecurityStampStore<TUser> GetSecurityStore()
//        {
//            var cast = Store as IUserSecurityStampStore<TUser>;
//            if (cast == null)
//            {
//                throw new NotSupportedException(Resources.StoreNotIUserSecurityStampStore);
//            }
//            return cast;
//        }

//        private IUserLockoutStore<TUser> GetUserLockoutStore()
//        {
//            var cast = Store as IUserLockoutStore<TUser>;
//            if (cast == null)
//            {
//                throw new NotSupportedException(Resources.StoreNotIUserLockoutStore);
//            }
//            return cast;
//        }
    }
}
