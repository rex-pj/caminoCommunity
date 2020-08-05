using Microsoft.AspNetCore.Mvc;

namespace Module.Web.Navigation.ViewComponents
{
    public class MainNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
