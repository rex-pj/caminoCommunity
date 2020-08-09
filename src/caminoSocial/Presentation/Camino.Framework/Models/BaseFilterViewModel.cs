namespace Camino.Framework.Models
{
    public class BaseFilterViewModel
    {
        public BaseFilterViewModel()
        {
            Page = 1;
            PageSize = 10;
        }

        public int PageSize { get; set; }
        public int Page { get; set; }
        
        public string Search { get; set; }
    }
}
