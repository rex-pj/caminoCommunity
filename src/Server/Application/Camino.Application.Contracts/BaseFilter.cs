namespace Camino.Application.Contracts
{
    public class BaseFilter
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string Keyword { get; set; }
    }
}
