using Camino.Business.Contracts;
using Camino.Core.Constants;
using Camino.Framework.Attributes;
using Camino.Framework.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Module.Web.AuthenticationManagement.Controllers
{
    public class UserStatusController : Controller
    {
        private readonly IUserStatusBusiness _userStatusBusiness;
        public UserStatusController(IUserStatusBusiness userStatusBusiness)
        {
            _userStatusBusiness = userStatusBusiness;
        }

        [HttpGet]
        [ApplicationAuthorize(AuthorizePolicyConst.CanReadUserStatus)]
        public IActionResult Search(string q)
        {
            var statuses = _userStatusBusiness.Search(q);
            if (statuses == null || !statuses.Any())
            {
                return Json(new
                {
                    Items = new List<Select2ItemModel>()
                });
            }

            var userModels = statuses
                .Select(x => new Select2ItemModel
                {
                    Id = x.Id.ToString(),
                    Text = x.Name
                });

            return Json(userModels);
        }
    }
}
