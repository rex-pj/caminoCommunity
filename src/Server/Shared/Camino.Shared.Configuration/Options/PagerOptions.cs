namespace Camino.Shared.Configuration.Options
{
    public class PagerOptions
    {
        public const string Name = "PagerOptions";
        public int AdvancedSearchPageSize { get; set; }
        public int LiveSearchPageSize { get; set; }
        public int PageSize { get; set; }
    }
}
