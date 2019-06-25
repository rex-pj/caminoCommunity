using System;
using System.Threading.Tasks;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.AccountIdentity.Entities;
using Coco.Api.Framework.Models;
using Microsoft.Extensions.Options;

namespace Coco.Api.Framework.AccountIdentity
{
    public class LoginManager : ILoginManager<ApplicationUser>
    {
        #region Fields/Properties
        //private readonly IRoleBusiness _roleBusiness;
        public IdentityOptions Options { get; set; }
        private readonly IAccountManager<ApplicationUser> _accountManager;
        #endregion
        #region Ctor
        public LoginManager(IAccountManager<ApplicationUser> accountManager, IOptions<IdentityOptions> optionsAccessor)
        {
            this.Options = optionsAccessor?.Value ?? new IdentityOptions();
            _accountManager = accountManager;
        }
        #endregion

        /// <summary>
        /// Attempts to sign in the specified <paramref name="userName"/> and <paramref name="password"/> combination
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="userName">The user name to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        public virtual async Task<ApiResult> LoginAsync(string userName, string password)
        {
            var user = await _accountManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ApiResult();
            }

            return await LoginAsync(user, password);
        }


        /// <summary>
        /// Attempts to sign in the specified <paramref name="user"/> and <paramref name="password"/> combination
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        public virtual async Task<ApiResult> LoginAsync(ApplicationUser user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var attempt = await CheckPasswordSignInAsync(user, password);
            return attempt;
        }


        /// <summary>
        /// Attempts a password sign in for a user.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        /// <returns></returns>
        public virtual async Task<ApiResult> CheckPasswordSignInAsync(ApplicationUser user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var result = await _accountManager.CheckPasswordAsync(user, password);

            return result;
        }
    }
}
