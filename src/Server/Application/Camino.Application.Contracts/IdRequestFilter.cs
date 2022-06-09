namespace Camino.Application.Contracts
{
    public class IdRequestFilter<TKey>
    {
        public TKey Id { get; set; }
        public bool CanGetDeleted { get; set; }
        public bool CanGetInactived { get; set; }
    }
}
