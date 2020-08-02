using Microsoft.AspNetCore.Mvc;

namespace Module.Web.Navigation.ViewComponents
{
    public class NavAccordionViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
