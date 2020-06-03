using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.SessionManager.Stores.Contracts;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager
{
    public class ApplicationUserManager<TUser> : UserManager<TUser>, IUserManager<TUser>, IDisposable where TUser : IdentityUser<long>
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

        private IUserEncryptionStore<TUser> GetUserEncryptionStore()
        {
            var cast = Store as IUserEncryptionStore<TUser>;
            if (cast == null)
            {
                throw new NotSupportedException("Store is not UserEncryptionStore");
            }
            return cast;
        }

        public async Task<string> EncryptUserIdAsync(long userId)
        {
            var userEncryptionStore = GetUserEncryptionStore();
            return await userEncryptionStore.EncryptUserId(userId);
        }

        public async Task<long> DecryptUserIdAsync(string userIdentityId)
        {
            var userEncryptionStore = GetUserEncryptionStore();
            return await userEncryptionStore.DecryptUserId(userIdentityId);
        }
    }
}
