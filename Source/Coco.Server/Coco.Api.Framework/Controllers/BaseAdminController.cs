using Microsoft.AspNetCore.Mvc;

namespace Coco.Api.Framework.Controllers
{
    [Area("Administrator")]
    public class BaseAdminController : ControllerBase
    {
        #region Fields/Properties
        private string _pageName;
        public string PageName { get; set; }
        #endregion

        #region Ctor
        public BaseAdminController()
        {
            _pageName = string.Empty;
        }
        #endregion
    }
}
