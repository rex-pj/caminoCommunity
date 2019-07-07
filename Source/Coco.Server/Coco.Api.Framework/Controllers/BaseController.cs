using Coco.Business.Contracts;
using Coco.Api.Framework.Models;
using Microsoft.AspNetCore.Mvc;
using Coco.Api.Framework.AccountIdentity;

namespace Coco.Api.Framework.Controllers
{
    public class BaseController : ControllerBase
    {
        #region Fields/Properties
        private AccountClaimsPrincipal _currentUser;
        public AccountClaimsPrincipal CurrentUser
        {
            get
            {
                _currentUser = base.User as AccountClaimsPrincipal;
                return _currentUser;
            }
            private set { _currentUser = value; }
        }
        #endregion

        #region Ctor
        public BaseController()
        {
            CurrentUser = base.User as AccountClaimsPrincipal;
        }
        #endregion
    }
}
