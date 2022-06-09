namespace Camino.Application.Contracts
{
    public class BasePageList<T> where T : class
    {
        public BasePageList(IEnumerable<T> collections)
        {
            Collections = collections;
        }

        public int TotalResult { get; set; }
        public int TotalPage { get; set; }
        public IEnumerable<T> Collections { get; set; }
    }
}
