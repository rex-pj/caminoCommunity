using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.ViewComponents
{
    public class NavAccordionViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
