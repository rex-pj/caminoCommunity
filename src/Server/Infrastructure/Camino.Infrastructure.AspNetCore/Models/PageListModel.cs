using Camino.Infrastructure.Identity.Models;

namespace Camino.Infrastructure.AspNetCore.Models
{
    public class PageListModel : BaseIdentityModel
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
