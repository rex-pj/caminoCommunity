using Camino.Framework.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Module.Web.NavigationManagement.ViewComponents
{
    public class PageNavigationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PageListModel paging)
        {
            return await Task.FromResult(View(paging));
        }
    }
}
