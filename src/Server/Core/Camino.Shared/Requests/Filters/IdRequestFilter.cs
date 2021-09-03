namespace Camino.Shared.Requests.Filters
{
    public class IdRequestFilter<TKey>
    {
        public TKey Id { get; set; }
        public bool CanGetDeleted { get; set; }
        public bool CanGetInactived { get; set; }
    }
}
