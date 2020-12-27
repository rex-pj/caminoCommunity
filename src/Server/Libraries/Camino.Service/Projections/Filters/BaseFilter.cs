namespace Camino.Service.Projections.Filters
{
    public class BaseFilter
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string Search { get; set; }
    }
}
