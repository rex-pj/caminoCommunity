using Camino.Framework.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Module.Web.Navigation.ViewComponents
{
    public class PageNavigationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PageListViewModel paging)
        {
            return await Task.FromResult(View(paging));
        }
    }
}
