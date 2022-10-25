using Microsoft.AspNetCore.Mvc;

namespace Module.Navigation.WebAdmin.ViewComponents
{
    public class TopNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
