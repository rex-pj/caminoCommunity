using Camino.Infrastructure.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Module.Navigation.WebAdmin.ViewComponents
{
    public class PageNavigationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PageListModel paging)
        {
            return await Task.FromResult(View(paging));
        }
    }
}
