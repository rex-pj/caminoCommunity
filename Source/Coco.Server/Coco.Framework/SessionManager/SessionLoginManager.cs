//using Coco.Framework.Models;
//using Coco.Framework.SessionManager.Contracts;
//using DescriberCore = Coco.Framework.SessionManager.Core;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Threading.Tasks;
//using System.Security.Claims;
//using System.Collections.Generic;
//using System.Linq;
//using Coco.Framework.SessionManager.Entities;
//using Coco.Framework.SessionManager.Stores.Contracts;

//namespace Coco.Framework.SessionManager
//{
//    public class SessionLoginManager : ILoginManager<ApplicationUser>
//    {
//        #region Fields/Properties
//        private readonly IUserManager<ApplicationUser> _userManager;
//        private HttpContext _context;
//        public DescriberCore.IdentityErrorDescriber Describer { get; private set; }
//        protected internal IUserStampStore<ApplicationUser> SecurityStampStore;
//        private readonly IHttpContextAccessor _contextAccessor;
//        private ISessionClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
//        public HttpContext Context
//        {
//            get
//            {
//                var context = _context ?? _contextAccessor?.HttpContext;
//                if (context == null)
//                {
//                    throw new InvalidOperationException("HttpContext must not be null.");
//                }
//                return context;
//            }
//            set
//            {
//                _context = value;
//            }
//        }

//        #endregion

//        #region Ctor
//        public SessionLoginManager(IUserManager<ApplicationUser> userManager,
//            IUserStampStore<ApplicationUser> securityStampStore,
//            IHttpContextAccessor httpContextAccessor,
//            ISessionClaimsPrincipalFactory<ApplicationUser> claimsFactory,
//            DescriberCore.IdentityErrorDescriber errors = null)
//        {
//            SecurityStampStore = securityStampStore;
//            _contextAccessor = httpContextAccessor;
//            _context = _contextAccessor?.HttpContext;
//            _userManager = userManager;
//            Describer = errors ?? new DescriberCore.IdentityErrorDescriber();
//            _claimsFactory = claimsFactory;
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
//                await Context.SignOutAsync(IdentitySettings.APP_SESSION_SCHEMA);
//                await LoginWithClaimsAsync(user, new AuthenticationProperties { IsPersistent = canRemember }, new Claim[] { new Claim("amr", "pwd") });
//                return result;
//            }

//            return CommonResult.Failed(Describer.PasswordMismatch());
//        }

//        public virtual async Task LoginWithClaimsAsync(ApplicationUser user, AuthenticationProperties authenticationProperties, IEnumerable<Claim> additionalClaims)
//        {
//            var userPrincipal = await _claimsFactory.CreateAsync(user);
//            foreach (var claim in additionalClaims)
//            {
//                userPrincipal.Identities.First().AddClaim(claim);
//            }
           
//            await Context.SignInAsync(IdentitySettings.APP_SESSION_SCHEMA,
//                userPrincipal, authenticationProperties);
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
//            await Context.SignOutAsync(IdentitySettings.APP_SESSION_SCHEMA);
//            return true;
//        }
//    }
//}
