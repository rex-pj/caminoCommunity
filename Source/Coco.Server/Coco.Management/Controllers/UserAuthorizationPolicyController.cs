using AutoMapper;
using Coco.Business.Contracts;
using Coco.Framework.Controllers;
using Coco.Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.Controllers
{
    public class UserAuthorizationPolicyController : BaseAuthController
    {
        private readonly IUserAuthorizationPolicyBusiness _userAuthorizationPolicyBusiness;
        private readonly IMapper _mapper;
        public UserAuthorizationPolicyController(IHttpContextAccessor httpContextAccessor, IUserAuthorizationPolicyBusiness userAuthorizationPolicyBusiness,
            IMapper mapper) : base(httpContextAccessor)
        {
            _userAuthorizationPolicyBusiness = userAuthorizationPolicyBusiness;
            _mapper = mapper;
        }

        public IActionResult Index(short id)
        {
            var result = _userAuthorizationPolicyBusiness.GetAuthoricationPolicyUsers(id);
            var authorizationUsers = _mapper.Map<AuthorizationPolicyUsersViewModel>(result);
            return View(authorizationUsers);
        }

        [HttpPost]
        public IActionResult Grant(AuthorizationPolicyUsersViewModel model)
        {
            var isSucceed = _userAuthorizationPolicyBusiness.Add(model.UserId, model.Id, LoggedUserId);
            if (isSucceed)
            {
                return RedirectToAction("Index", new { id = model.Id });
            }
            return RedirectToAction("Index", new { id = model.Id });
        }

        [HttpPost]
        public IActionResult Ungrant(long userId, short authorizationPolicyId)
        {
            var isSucceed = _userAuthorizationPolicyBusiness.Delete(userId, authorizationPolicyId);
            if (isSucceed)
            {
                return RedirectToAction("Index", new { id = authorizationPolicyId });
            }
            return RedirectToAction("Index", new { id = authorizationPolicyId });
        }
    }
}