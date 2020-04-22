using System;
using System.Threading.Tasks;
using Coco.Framework.SessionManager.Contracts;
using Coco.Framework.Models;
using Microsoft.Extensions.Options;
using Coco.Framework.SessionManager.Core;

namespace Coco.Framework.SessionManager
{
    public class LoginManager : ILoginManager<ApplicationUser>
    {
        #region Fields/Properties
        public IdentityOptions Options { get; set; }
        private readonly IUserManager<ApplicationUser> _userManager;
        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.</value>
        public IdentityErrorDescriber Describer { get; private set; }
        protected internal IUserStampStore<ApplicationUser> SecurityStampStore;

        #endregion

        #region Ctor
        public LoginManager(IUserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor,
            IUserStampStore<ApplicationUser> securityStampStore,
            IdentityErrorDescriber errors = null)
        {
            SecurityStampStore = securityStampStore;
            this.Options = optionsAccessor?.Value ?? new IdentityOptions();
            _userManager = userManager;
            Describer = errors ?? new IdentityErrorDescriber();
        }
        #endregion

        /// <summary>
        /// Attempts to sign in the specified <paramref name="userName"/> and <paramref name="password"/> combination
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="userName">The user name to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user user should be locked if the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        public virtual async Task<IApiResult> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var result = await CheckPasswordSignInAsync(user, password);
            if (result.IsSucceed)
            {
                return result;
            }

            return ApiResult.Failed(Describer.PasswordMismatch());
        }

        /// <summary>
        /// Attempts a password sign in for a user.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        /// <returns></returns>
        public virtual async Task<IApiResult> CheckPasswordSignInAsync(ApplicationUser user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.PasswordSalt = await SecurityStampStore.GetPasswordSaltAsync(user.Id);
            user.PasswordHash = user.Password;
            return await _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="userIdentity">The user id is encrypted</param>
        /// <param name="authenticationToken">Jwt token</param>
        /// <returns></returns>
        public virtual async Task<bool> LogoutAsync(string userIdentityId, string authenticationToken)
        {
            if (string.IsNullOrEmpty(userIdentityId))
            {
                throw new ArgumentNullException(nameof(userIdentityId));
            }

            if (string.IsNullOrEmpty(authenticationToken))
            {
                throw new ArgumentNullException(nameof(authenticationToken));
            }

            return await _userManager.ClearUserLoginAsync(userIdentityId, authenticationToken);
        }
    }
}
