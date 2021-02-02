namespace Camino.Service.Projections.Filters
{
    public class SelectFilter
    {
        public SelectFilter()
        {
            Search = string.Empty;
        }

        public long CreatedById { get; set; }
        public long[] CurrentIds { get; set; }
        public string Search { get; set; }
    }
}
