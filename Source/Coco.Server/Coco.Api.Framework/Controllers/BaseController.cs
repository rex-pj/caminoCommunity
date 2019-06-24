using Coco.Business.Contracts;
using Coco.Api.Framework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Api.Framework.Controllers
{
    public class BaseController : ControllerBase
    {
        #region Fields/Properties
        private string _pageName;
        protected string PageName { get; set; }
        #endregion

        #region Ctor
        public BaseController(
            )
        {
            _pageName = string.Empty;
        }
        #endregion
    }
}
