using System;
using System.Threading.Tasks;
using Coco.Api.Framework.UserIdentity.Contracts;
using Coco.Api.Framework.UserIdentity.Entities;
using Coco.Api.Framework.Models;
using Microsoft.Extensions.Options;

namespace Coco.Api.Framework.UserIdentity
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
        #endregion
        #region Ctor
        public LoginManager(IUserManager<ApplicationUser> userManager, 
            IOptions<IdentityOptions> optionsAccessor,
            IdentityErrorDescriber errors = null)
        {
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
        public virtual async Task<ApiResult> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ApiResult();
            }

            var result = await LoginAsync(user, password);

            if (result.IsSuccess)
            {
                return result;
            }
            return ApiResult<UserTokenResult>.Failed(Describer.PasswordMismatch(), new UserTokenResult());
        }


        /// <summary>
        /// Attempts to sign in the specified <paramref name="user"/> and <paramref name="password"/> combination
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user user should be locked if the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        public virtual async Task<ApiResult> LoginAsync(ApplicationUser user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await CheckPasswordSignInAsync(user, password);
        }


        /// <summary>
        /// Attempts a password sign in for a user.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user user should be locked if the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        /// <returns></returns>
        public virtual async Task<ApiResult> CheckPasswordSignInAsync(ApplicationUser user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.PasswordHash = user.Password;
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
