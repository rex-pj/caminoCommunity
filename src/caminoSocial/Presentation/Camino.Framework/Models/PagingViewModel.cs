namespace Camino.Framework.Models
{
    public class PagingViewModel
    {
        public PagingViewModel()
        {
            Page = 1;
            PageSize = 10;
        }

        public int PageSize { get; set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int Page { get; set; }
        public int TotalResult { get; set; }
        public int TotalPage { get; set; }
    }
}
