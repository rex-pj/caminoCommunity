using Camino.Framework.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Module.Web.Navigation.ViewComponents
{
    public class PageNavigation : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PagingViewModel paging)
        {
            return await Task.FromResult(View(paging));
        }
    }
}
