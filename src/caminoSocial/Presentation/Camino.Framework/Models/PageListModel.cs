using System.Collections.Generic;

namespace Camino.Framework.Models
{
    public class PageListModel : BaseModel
    {
        public PageListModel()
        {
            Filter = new BaseFilterModel();
        }

        public int TotalResult { get; set; }
        public int TotalPage { get; set; }
        public BaseFilterModel Filter { get; set; }
    }

    public class PageListModel<T> : PageListModel where T : class
    {
        public PageListModel(IEnumerable<T> collections) : base()
        {
            Collections = collections;
        }

        public IEnumerable<T> Collections { get; set; }
    }
}
