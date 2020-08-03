using System.Collections.Generic;

namespace Camino.Framework.Models
{
    public class PageListViewModel<T> : BaseViewModel where T : class
    {
        public PageListViewModel(IEnumerable<T> collections)
        {
            Paging = new PagingViewModel();
            SearchCriteria = new SearchCriteriaViewModel();
            Collections = collections;
        }

        public PagingViewModel Paging { get; set; }
        public SearchCriteriaViewModel SearchCriteria { get; set; }
        public IEnumerable<T> Collections { get; set; }
    }
}
