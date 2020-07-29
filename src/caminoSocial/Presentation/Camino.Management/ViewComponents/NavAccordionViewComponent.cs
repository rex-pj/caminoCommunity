using Microsoft.AspNetCore.Mvc;

namespace Camino.Management.ViewComponents
{
    public class NavAccordionViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
