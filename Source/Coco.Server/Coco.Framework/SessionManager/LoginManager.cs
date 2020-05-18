//using System;
//using System.Threading.Tasks;
//using Coco.Framework.SessionManager.Contracts;
//using Coco.Framework.Models;
//using Microsoft.Extensions.Options;
//using Coco.Framework.SessionManager.Core;
//using Coco.Framework.SessionManager.Stores.Contracts;
//using Microsoft.AspNetCore.Identity;

//namespace Coco.Framework.SessionManager
//{
//    public class LoginManager : ILoginManager<ApplicationUser>
//    {
//        #region Fields/Properties
//        public IdentityOptions Options { get; set; }
//        private readonly IUserManager<ApplicationUser> _userManager;
//        /// <summary>
//        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.
//        /// </summary>
//        /// <value>The <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.</value>
//        public IdentityErrorDescriber Describer { get; private set; }
//        protected internal IUserStampStore<ApplicationUser> SecurityStampStore;

//        #endregion

//        #region Ctor
//        public LoginManager(IUserManager<ApplicationUser> userManager,
//            IOptions<IdentityOptions> optionsAccessor,
//            IUserStampStore<ApplicationUser> securityStampStore,
//            IdentityErrorDescriber errors = null)
//        {
//            SecurityStampStore = securityStampStore;
//            this.Options = optionsAccessor?.Value ?? new IdentityOptions();
//            _userManager = userManager;
//            Describer = errors ?? new IdentityErrorDescriber();
//        }
//        #endregion

//        public virtual async Task<ICommonResult> LoginAsync(string userName, string password, bool canRemember = true)
//        {
//            var user = await _userManager.FindByNameAsync(userName);
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            var result = await CheckPasswordSignInAsync(user, password);
//            if (result.IsSucceed)
//            {
//                return result;
//            }

//            return CommonResult.Failed(Describer.PasswordMismatch());
//        }

//        public virtual async Task<ICommonResult> CheckPasswordSignInAsync(ApplicationUser user, string password)
//        {
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            user.PasswordSalt = await SecurityStampStore.GetPasswordSaltAsync(user.Id);
//            user.PasswordHash = user.Password;
//            return await _userManager.CheckPasswordAsync(user, password);
//        }

//        public virtual async Task<bool> LogoutAsync(ApplicationUser user = null)
//        {
//            if(user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            if (string.IsNullOrEmpty(user.UserIdentityId))
//            {
//                throw new ArgumentNullException(nameof(user.UserIdentityId));
//            }

//            if (string.IsNullOrEmpty(user.AuthenticationToken))
//            {
//                throw new ArgumentNullException(nameof(user.AuthenticationToken));
//            }

//            return await _userManager.ClearUserLoginAsync(user.UserIdentityId, user.AuthenticationToken);
//        }
//    }
//}
