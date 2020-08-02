using Microsoft.AspNetCore.Mvc;

namespace Module.Web.Navigation.ViewComponents
{
    public class TopNavViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
