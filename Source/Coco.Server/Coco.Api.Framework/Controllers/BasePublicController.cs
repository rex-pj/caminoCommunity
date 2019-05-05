using Coco.Business.Contracts;
using Coco.Api.Framework.Models;
using Microsoft.AspNetCore.Identity;

namespace Coco.Api.Framework.Controllers
{
    public class BasePublicController : BaseController
    {
        public BasePublicController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IRoleBusiness roleBusiness)
            : base(userManager, signInManager, roleBusiness) { }
    }
}