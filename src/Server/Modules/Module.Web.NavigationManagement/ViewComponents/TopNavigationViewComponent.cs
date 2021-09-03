using Microsoft.AspNetCore.Mvc;

namespace Module.Web.NavigationManagement.ViewComponents
{
    public class TopNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
