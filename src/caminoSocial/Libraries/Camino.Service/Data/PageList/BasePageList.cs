using System.Collections.Generic;
using Camino.Service.Data.Filters;

namespace Camino.Service.Data.PageList
{
    public class BasePageList<T> where T : class
    {
        public BasePageList(IEnumerable<T> collections)
        {
            Collections = collections;
            Filter = new BaseFilter();
        }

        public BaseFilter Filter { get; set; }
        public int TotalResult { get; set; }
        public int TotalPage { get; set; }
        public IEnumerable<T> Collections { get; set; }
    }
}
