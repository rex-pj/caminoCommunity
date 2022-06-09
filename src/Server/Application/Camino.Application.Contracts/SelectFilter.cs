namespace Camino.Application.Contracts
{
    public class SelectFilter
    {
        public SelectFilter()
        {
            Keyword = string.Empty;
            CurrentIds = new long[0];
        }

        public long CreatedById { get; set; }
        public long[] CurrentIds { get; set; }
        public string Keyword { get; set; }
    }
}
