using System.Collections.Generic;

namespace Camino.Framework.Models
{
    public class PagerViewModel<T> where T : class
    {
        public PagerViewModel()
        {
            PageNumber = 1;
            PageSize = 10;
            Collections = new List<T>();
        }

        public PagerViewModel(IEnumerable<T> collections)
        {
            PageNumber = 1;
            PageSize = 10;
            Collections = collections;
        }

        public int PageSize { get; set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int PageNumber { get; set; }

        public IEnumerable<T> Collections { get; set; }
    }
}
