using Microsoft.AspNetCore.Mvc;

namespace Module.Web.Navigation.ViewComponents
{
    public class TopNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
