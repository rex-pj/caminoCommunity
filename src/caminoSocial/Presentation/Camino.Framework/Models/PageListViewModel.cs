using System.Collections.Generic;

namespace Camino.Framework.Models
{
    public class PageListViewModel : BaseViewModel
    {
        public PageListViewModel()
        {
            Filter = new BaseFilterViewModel();
        }

        public int TotalResult { get; set; }
        public int TotalPage { get; set; }
        public BaseFilterViewModel Filter { get; set; }
    }

    public class PageListViewModel<T> : PageListViewModel where T : class
    {
        public PageListViewModel(IEnumerable<T> collections) : base()
        {
            Collections = collections;
        }

        public IEnumerable<T> Collections { get; set; }
    }
}
