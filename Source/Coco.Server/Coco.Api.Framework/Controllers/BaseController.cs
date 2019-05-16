using Coco.Business.Contracts;
using Coco.Api.Framework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Api.Framework.Controllers
{
    public class BaseController : ControllerBase
    {
        //protected readonly UserManager<ApplicationUser> _userManager;
        //protected readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRoleBusiness _roleBusiness;

        #region Fields/Properties
        private string _pageName;
        protected string PageName { get; set; }
        //protected virtual new ApplicationClaimsPrincipal User
        //{
        //    get
        //    {
        //        return new ApplicationClaimsPrincipal(base.User, _roleBusiness);
        //    }
        //}
        #endregion

        #region Ctor
        public BaseController(
            //UserManager<ApplicationUser> userManager, 
            //SignInManager<ApplicationUser> signInManager
            //, IRoleBusiness roleBusiness
            )
        {
            _pageName = string.Empty;
            //_userManager = userManager;
            //_signInManager = signInManager;
            //_roleBusiness = roleBusiness;
        }

        //public BaseController()
        //{
        //    _pageName = string.Empty;
        //}
        #endregion
    }
}
